using GovUK.Dfe.CoreLibs.ApplicationSettings.Configuration;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Data;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Interfaces;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GovUK.Dfe.CoreLibs.ApplicationSettings.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationSettings(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName = "DefaultConnection",
        string? schema = null)
    {
        // Configure options with defaults and optional schema override
        services.Configure<ApplicationSettingsOptions>(options =>
        {
            // Set defaults
            options.EnableCaching = true;
            options.CacheExpirationMinutes = 30;
            options.DefaultCategory = "General";
            options.EnableEncryption = false;
            options.Schema = schema; // Use provided schema or null for default

            // Try to read from configuration section if it exists
            var section = configuration.GetSection(ApplicationSettingsOptions.ConfigurationSection);
            if (section.Exists())
            {
                if (bool.TryParse(section["EnableCaching"], out bool enableCaching))
                    options.EnableCaching = enableCaching;

                if (int.TryParse(section["CacheExpirationMinutes"], out int cacheExpiration))
                    options.CacheExpirationMinutes = cacheExpiration;

                if (!string.IsNullOrEmpty(section["DefaultCategory"]))
                    options.DefaultCategory = section["DefaultCategory"];

                if (bool.TryParse(section["EnableEncryption"], out bool enableEncryption))
                    options.EnableEncryption = enableEncryption;

                if (!string.IsNullOrEmpty(section["EncryptionKey"]))
                    options.EncryptionKey = section["EncryptionKey"];

                // Schema from configuration (only if not overridden by parameter)
                if (schema == null && !string.IsNullOrEmpty(section["Schema"]))
                    options.Schema = section["Schema"];
            }
        });

        // Add DbContext
        services.AddDbContext<ApplicationSettingsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(connectionStringName)));

        // Add memory cache if not already added
        services.AddMemoryCache();

        // Add the service
        services.AddScoped<IApplicationSettingsService, ApplicationSettingsService>();

        return services;
    }

    /// <summary>
    /// Adds ApplicationSettings service using an existing DbContext
    /// </summary>
    /// <typeparam name="TContext">The existing DbContext type</typeparam>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <param name="schema">Optional schema override</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddApplicationSettingsWithExistingContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? schema = null)
        where TContext : DbContext
    {
        // Configure options
        services.Configure<ApplicationSettingsOptions>(options =>
        {
            options.EnableCaching = true;
            options.CacheExpirationMinutes = 30;
            options.DefaultCategory = "General";
            options.EnableEncryption = false;
            options.Schema = schema;

            // Read from configuration
            var section = configuration.GetSection(ApplicationSettingsOptions.ConfigurationSection);
            if (section.Exists())
            {
                if (bool.TryParse(section["EnableCaching"], out bool enableCaching))
                    options.EnableCaching = enableCaching;

                if (int.TryParse(section["CacheExpirationMinutes"], out int cacheExpiration))
                    options.CacheExpirationMinutes = cacheExpiration;

                // Update the line to use the null-coalescing operator to handle potential null values
                if (!string.IsNullOrEmpty(section["DefaultCategory"]))
                    options.DefaultCategory = section["DefaultCategory"] ?? string.Empty;

                if (bool.TryParse(section["EnableEncryption"], out bool enableEncryption))
                    options.EnableEncryption = enableEncryption;

                if (!string.IsNullOrEmpty(section["EncryptionKey"]))
                    options.EncryptionKey = section["EncryptionKey"];

                if (schema == null && !string.IsNullOrEmpty(section["Schema"]))
                    options.Schema = section["Schema"];
            }
        });

        // Add memory cache if not already added
        services.AddMemoryCache();

        // Add custom service implementation that uses the existing context
        services.AddScoped<IApplicationSettingsService, ExistingContextApplicationSettingsService<TContext>>();

        return services;
    }

    public static IServiceCollection AddApplicationSettings(
        this IServiceCollection services,
        string connectionString,
        Action<ApplicationSettingsOptions>? configureOptions = null)
    {
        // Configure options with defaults and optional customization
        services.Configure<ApplicationSettingsOptions>(options =>
        {
            // Set defaults
            options.EnableCaching = true;
            options.CacheExpirationMinutes = 30;
            options.DefaultCategory = "General";
            options.EnableEncryption = false;
            options.Schema = null; // Default to no schema

            // Apply custom configuration if provided
            configureOptions?.Invoke(options);
        });

        // Add DbContext
        services.AddDbContext<ApplicationSettingsDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Add memory cache if not already added
        services.AddMemoryCache();

        // Add the service
        services.AddScoped<IApplicationSettingsService, ApplicationSettingsService>();

        return services;
    }
}