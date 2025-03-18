# Set the major version of dotnet
ARG DOTNET_VERSION=8.0
# Set the major version of nodejs
ARG NODEJS_VERSION_MAJOR=22

# ==============================================
# Assets Build Stage (Node.js)
# ==============================================
FROM node:${NODEJS_VERSION_MAJOR}-bullseye-slim AS assets
WORKDIR /app
COPY ./src/Dfe.ManageSchoolImprovement/wwwroot .
RUN npm ci --ignore-scripts && npm run postinstall && npm run build

# ==============================================
# .NET SDK Build Stage
# ==============================================
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0 AS build
WORKDIR /build

# Copy solution and props files
COPY ./Dfe.ManageSchoolImprovement.sln .
COPY ./Directory.Build.props .

## START: Restore Packages
ARG PROJECT_NAME="Dfe.ManageSchoolImprovement"
# Copy csproj files for restore caching
COPY ./src/${PROJECT_NAME}.Application/${PROJECT_NAME}.Application.csproj                         ./src/${PROJECT_NAME}.Application/
COPY ./src/${PROJECT_NAME}.Domain/${PROJECT_NAME}.Domain.csproj                                   ./src/${PROJECT_NAME}.Domain/
COPY ./src/${PROJECT_NAME}.Infrastructure/${PROJECT_NAME}.Infrastructure.csproj                   ./src/${PROJECT_NAME}.Infrastructure/
COPY ./src/${PROJECT_NAME}.Utils/${PROJECT_NAME}.Utils.csproj                                     ./src/${PROJECT_NAME}.Utils/
COPY ./src/${PROJECT_NAME}/${PROJECT_NAME}.Frontend.csproj                                        ./src/${PROJECT_NAME}/
COPY ./src/Tests/${PROJECT_NAME}.Application.Tests/${PROJECT_NAME}.Application.Tests.csproj       ./src/Tests/${PROJECT_NAME}.Application.Tests/
COPY ./src/Tests/${PROJECT_NAME}.Domain.Tests/${PROJECT_NAME}.Domain.Tests.csproj                 ./src/Tests/${PROJECT_NAME}.Domain.Tests/
COPY ./src/Tests/${PROJECT_NAME}.Frontend.Tests/${PROJECT_NAME}.Frontend.Tests.csproj             ./src/Tests/${PROJECT_NAME}.Frontend.Tests/
COPY ./src/Tests/${PROJECT_NAME}.Infrastructure.Tests/${PROJECT_NAME}.Infrastructure.Tests.csproj ./src/Tests/${PROJECT_NAME}.Infrastructure.Tests/
COPY ./src/Tests/${PROJECT_NAME}.Tests.Common/${PROJECT_NAME}.Tests.Common.csproj                 ./src/Tests/${PROJECT_NAME}.Tests.Common/
COPY ./src/Tests/${PROJECT_NAME}.Utils.Tests/${PROJECT_NAME}.Utils.Tests.csproj                   ./src/Tests/${PROJECT_NAME}.Utils.Tests/

# Mount GitHub Token and restore
RUN --mount=type=secret,id=github_token dotnet nuget add source --username USERNAME --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json" && \
    dotnet restore ${PROJECT_NAME}.sln
## END: Restore Packages

COPY ./src/ /build/src/
# Build and publish
RUN dotnet build ${PROJECT_NAME}.sln --no-restore -c Release && \
    dotnet publish ./src/${PROJECT_NAME}/${PROJECT_NAME}.Frontend.csproj \
      -c Release \
      -o /app \
      --no-build

# Copy entrypoint script
COPY ./scripts/docker-entrypoint.sh /app/docker-entrypoint.sh

# ==============================================
# Entity Framework: Migration Builder
# ==============================================
FROM build AS efbuilder
WORKDIR /build

# Install dotnet-ef and create migration bundle
RUN mkdir /sql && \
    dotnet new tool-manifest && \
    dotnet tool install dotnet-ef --version 8.0.13 && \
    dotnet ef migrations bundle \
      -r linux-x64 \
      -p src/Dfe.ManageSchoolImprovement.Infrastructure \
      --configuration Release \
      --no-build \
      -o /sql/migratedb

# ==============================================
# Entity Framework: Migration Runner
# ==============================================
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0 AS initcontainer
WORKDIR /sql
COPY --from=efbuilder /app/appsettings.json /Dfe.ManageSchoolImprovement/
COPY --from=efbuilder /sql /sql

# Set ownership and switch user
RUN chown "$APP_UID" /sql -R
USER $APP_UID

# ==============================================
# .NET: Runtime
# ==============================================
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0 AS final
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/manage-school-improvement"

COPY --from=build /app .
COPY --from=assets /app ./wwwroot

# Set permissions and user
RUN chmod +x ./docker-entrypoint.sh
USER $APP_UID
