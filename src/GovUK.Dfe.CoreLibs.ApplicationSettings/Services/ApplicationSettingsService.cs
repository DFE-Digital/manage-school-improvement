using GovUK.Dfe.CoreLibs.ApplicationSettings.Configuration;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Data;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Entities;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GovUK.Dfe.CoreLibs.ApplicationSettings.Services;

public class ApplicationSettingsService : IApplicationSettingsService
{
    private readonly ApplicationSettingsDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly ApplicationSettingsOptions _options;
    private readonly ILogger<ApplicationSettingsService> _logger;

    private const string CacheKeyPrefix = "AppSettings_";
    private const string AllSettingsCacheKey = "AppSettings_All";

    public ApplicationSettingsService(
        ApplicationSettingsDbContext context,
        IMemoryCache cache,
        IOptions<ApplicationSettingsOptions> options,
        ILogger<ApplicationSettingsService> logger)
    {
        _context = context;
        _cache = cache;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string?> GetSettingAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Setting key cannot be null or empty", nameof(key));

        var cacheKey = $"{CacheKeyPrefix}{key}";

        if (_options.EnableCaching && _cache.TryGetValue(cacheKey, out string? cachedValue))
        {
            return cachedValue;
        }

        var setting = await _context.ApplicationSettings
            .FirstOrDefaultAsync(s => s.Key == key && s.IsActive, cancellationToken);

        var value = setting?.Value;

        if (_options.EnableCaching && value != null)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheExpirationMinutes)
            };
            _cache.Set(cacheKey, value, cacheOptions);
        }

        return value;
    }

    public async Task<T?> GetSettingAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var value = await GetSettingAsync(key, cancellationToken);
        if (value == null) return null;

        try
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize setting {Key} to type {Type}", key, typeof(T).Name);
            return null;
        }
    }

    public async Task<string> GetSettingAsync(string key, string defaultValue, CancellationToken cancellationToken = default)
    {
        var value = await GetSettingAsync(key, cancellationToken);
        return value ?? defaultValue;
    }

    public async Task<Dictionary<string, string>> GetSettingsByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be null or empty", nameof(category));

        var cacheKey = $"{CacheKeyPrefix}Category_{category}";

        if (_options.EnableCaching && _cache.TryGetValue(cacheKey, out Dictionary<string, string>? cachedSettings))
        {
            return cachedSettings!;
        }

        var settings = await _context.ApplicationSettings
            .Where(s => s.Category == category && s.IsActive)
            .ToDictionaryAsync(s => s.Key, s => s.Value, cancellationToken);

        if (_options.EnableCaching)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheExpirationMinutes)
            };
            _cache.Set(cacheKey, settings, cacheOptions);
        }

        return settings;
    }

    public async Task<Dictionary<string, string>> GetAllSettingsAsync(CancellationToken cancellationToken = default)
    {
        if (_options.EnableCaching && _cache.TryGetValue(AllSettingsCacheKey, out Dictionary<string, string>? cachedAllSettings))
        {
            return cachedAllSettings!;
        }

        var settings = await _context.ApplicationSettings
            .Where(s => s.IsActive)
            .ToDictionaryAsync(s => s.Key, s => s.Value, cancellationToken);

        if (_options.EnableCaching)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheExpirationMinutes)
            };
            _cache.Set(AllSettingsCacheKey, settings, cacheOptions);
        }

        return settings;
    }

    public async Task SetSettingAsync(string key, string value, string? description = null, string? category = null, string? updatedBy = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Setting key cannot be null or empty", nameof(key));

        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var setting = await _context.ApplicationSettings
            .FirstOrDefaultAsync(s => s.Key == key, cancellationToken);

        if (setting == null)
        {
            setting = new ApplicationSetting
            {
                Key = key,
                Value = value,
                Description = description,
                Category = category ?? _options.DefaultCategory,
                CreatedBy = updatedBy,
                UpdatedBy = updatedBy
            };
            _context.ApplicationSettings.Add(setting);
        }
        else
        {
            setting.Value = value;
            setting.Description = description ?? setting.Description;
            setting.Category = category ?? setting.Category;
            setting.UpdatedAt = DateTime.UtcNow;
            setting.UpdatedBy = updatedBy;
            setting.IsActive = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate cache
        if (_options.EnableCaching)
        {
            _cache.Remove($"{CacheKeyPrefix}{key}");
            _cache.Remove($"{CacheKeyPrefix}Category_{setting.Category}");
            _cache.Remove(AllSettingsCacheKey);
        }

        _logger.LogInformation("Setting {Key} updated by {UpdatedBy}", key, updatedBy ?? "System");
    }

    public async Task SetSettingsAsync(Dictionary<string, string> settings, string? category = null, string? updatedBy = null, CancellationToken cancellationToken = default)
    {
        if (settings == null || !settings.Any())
            return;

        foreach (var kvp in settings)
        {
            await SetSettingAsync(kvp.Key, kvp.Value, null, category, updatedBy, cancellationToken);
        }
    }

    public async Task DeleteSettingAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Setting key cannot be null or empty", nameof(key));

        var setting = await _context.ApplicationSettings
            .FirstOrDefaultAsync(s => s.Key == key, cancellationToken);

        if (setting != null)
        {
            setting.IsActive = false;
            setting.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            // Invalidate cache
            if (_options.EnableCaching)
            {
                _cache.Remove($"{CacheKeyPrefix}{key}");
                _cache.Remove($"{CacheKeyPrefix}Category_{setting.Category}");
                _cache.Remove(AllSettingsCacheKey);
            }

            _logger.LogInformation("Setting {Key} deleted", key);
        }
    }

    public async Task<bool> SettingExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        return await _context.ApplicationSettings
            .AnyAsync(s => s.Key == key && s.IsActive, cancellationToken);
    }

    public async Task RefreshCacheAsync(CancellationToken cancellationToken = default)
    {
        if (!_options.EnableCaching) return;

        // Clear all cached settings
        var allSettings = await _context.ApplicationSettings
            .Where(s => s.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var setting in allSettings)
        {
            var cacheKey = $"{CacheKeyPrefix}{setting.Key}";
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheExpirationMinutes)
            };
            _cache.Set(cacheKey, setting.Value, cacheOptions);
        }

        _logger.LogInformation("Application settings cache refreshed with {Count} settings", allSettings.Count);
    }
}