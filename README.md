
# Manage school improvement

## Overview

This project is a .NET Razor application designed to assist in managing school improvement initiatives within the RISE policy. It integrates a Node.js-based build pipeline to handle frontend CSS, utilizing modern tools like Webpack and Sass for building and optimizing CSS assets used by the Razor views.

---


## Features

- **.NET Razor Application**: Leverages the power of Razor for dynamic web content rendering.
- **Node.js Build Pipeline**: Employs Node.js tools to manage and optimize frontend assets.
- **Webpack & Sass Integration**: Utilizes Webpack and Sass for efficient and maintainable CSS development.

## Prerequisites

Before getting started, ensure the following tools are installed on your system:

1. **.NET SDK** (version 8.0 or higher)
2. **Node.js** (version 16 or higher)
3. **NPM** (comes with Node.js installation)

---

## Setup Instructions

### Install Dependencies

#### Entity framework tools for managing data migrations:
Run the following command to install ef tools:
```bash
dotnet tool install --global dotnet-ef
```
#### Local database:
Make sure you have docker running and run the following command to pull the latest version mssql
```bash
docker pull mcr.microsoft.com/azure-sql-edge:latest
```

Now start the container and set the required settings
```bash
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=StrongPassword905' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
```

#### DfE Nuget package source:
This project requires packages from the RSD DfE nuget repository, go [here](https://github.com/DFE-Digital/academisation-nuget-packages) for more information on how to det that up.

---

## Getting Started

Follow these steps to set up the project locally:

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/DFE-Digital/manage-school-improvement.git
   ```

2. **Navigate to the Project Directory**:
   ```bash
   cd manage-school-improvement
   ```
3. **Restore .NET Dependencies**:
   ```bash
   dotnet restore
   ```

4. **Navigate to the wwwroot Directory**:
   ```bash
   cd .\src\Dfe.ManageSchoolImprovement\wwwroot
   ```

5. **Install Node.js Dependencies**:
   ```bash
   npm install
   ```

6. **Build Frontend Assets**:
   ```bash
   npm run build
   ```
7. **Navigate back to the Project Directory**:
   ```bash
   cd manage-school-improvement
   ```
8. **Update the database to the latest migration by running this command**:
    ```bash
    dotnet ef database update --connection "Server=localhost,1433;Database=sip;User=sa;Password=StrongPassword905;TrustServerCertificate=True" --startup-project .\src\Dfe.ManageSchoolImprovement\ --project .\src\Dfe.ManageSchoolImprovement.Infrastructure\ --context RegionalImprovementForStandardsAndExcellenceContext
    ```
9. **Update your secrets**:
    Ask for help from another dev on the project to get the right values for this file
    ```bash
    {
      "AcademiesApi": {
        "ApiKey": "API-KEY-HERE",
        "Url": "https://api.dev.academies.education.gov.uk/"
      },
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=sip;User ID=sa;Password=StrongPassword905;TrustServerCertificate=True"
      },
      "AzureAd": {
        "ClientId": "CLIENT-ID-HERE",
        "ClientSecret": "CLIENT-SECRET-HERE",
        "TenantId": "TENANT-ID-HERE",
        "AllowedRoles": "msi.edit",
        "GroupId": "GROUP-ID-HERE"
      }
    }
    ```

10. **Run the Application**:
    ```bash
    dotnet run
    ```

   The application should now be running at `http://localhost:7088`.

---

## NPM Scripts

- **`npm run build:watch`**  
  Watches CSS files, rebuilds on changes, and copies the output to the `wwwroot/css` directory.

- **`npm run build`**  
  Builds and optimizes CSS for production.

- **`npm run clean`**  
  Cleans the `dist` folder and resets build files.

--

---

## Troubleshooting

- **CSS not updating:**  
  Run `npm run build` icommand in the `wwwroot` directory and check that the output files are being copied to `/wwwroot/src`.

- **Dependency issues:**  
  Check your Node.js or .NET SDK versions and ensure they match the prerequisites.

---

### Linting Sonar rules

Include the following extension in your IDE installation: [SonarQube for IDE](https://marketplace.visualstudio.com/items?itemName=SonarSource.sonarlint-vscode)

Update your [settings.json file](https://code.visualstudio.com/docs/getstarted/settings#_settings-json-file) to include the following

```json
"sonarlint.connectedMode.connections.sonarcloud": [   
    {
        "connectionId": "DfE",
        "organizationKey": "dfe-digital",
        "disableNotifications": false
    }   
]
```

Then follow [these steps](https://youtu.be/m8sAdYCIWhY) to connect to the SonarCloud instance.

## Contributions

Feel free to submit issues or pull requests to improve this project. 

---

## License

This project is licensed under the [MIT License](LICENSE).
