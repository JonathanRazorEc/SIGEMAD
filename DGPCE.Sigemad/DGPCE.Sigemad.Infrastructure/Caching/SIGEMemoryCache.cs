using DGPCE.Sigemad.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DGPCE.Sigemad.Infrastructure.Caching;
internal class SIGEMemoryCache : ISIGEMemoryCache
{
    private const int DEFAULT_ABSOLUTE_EXPIRATION = 1440; // 24 horas
    private const int DEFAULT_SLIDING_EXPIRATION = 10; // 10 min
    private const int DEFAULT_LOCAL_EXPIRATION = 300; // 5 min

    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<SIGEMemoryCache> _logger;

    private readonly int _defaultMinutesToExpire;
    private readonly int _absoluteExpiration;
    private readonly int _slidingExpiration;
    private readonly int _localExpiration;

    private readonly Dictionary<string, int> _typeExpirations;

    public SIGEMemoryCache(IMemoryCache memoryCache, ILogger<SIGEMemoryCache> logger, IConfiguration config)
    {
        _memoryCache = memoryCache;
        _logger = logger;

        _absoluteExpiration = int.TryParse(config["Cache:AbsoluteExpiration"], out var abs) ? abs : DEFAULT_ABSOLUTE_EXPIRATION;
        _slidingExpiration = int.TryParse(config["Cache:SlidingExpiration"], out var slid) ? slid : DEFAULT_SLIDING_EXPIRATION;
        _localExpiration = int.TryParse(config["Cache:ExpirationRelativeToNowLocalCache"], out var local) ? local : DEFAULT_LOCAL_EXPIRATION;

        _defaultMinutesToExpire = int.TryParse(config["Cache:DefaultMinutesToExpire"], out var minutes)
            ? minutes : 10;

        _typeExpirations = config.GetSection("Cache:Expirations")?.GetChildren()?.Any() == true
            ? config.GetSection("Cache:Expirations")
            .GetChildren()
            .Where(x => int.TryParse(x.Value, out _))
            .ToDictionary(x => x.Key, x => int.Parse(x.Value!))
            : new Dictionary<string, int>();

    }

    public Task<T?> GetCacheAsync<T>(string key, CancellationToken token = default) where T : class
    {
        if (_memoryCache.TryGetValue(key, out T value))
        {
            _logger.LogDebug("Cache hit: {Key}", key);
            return Task.FromResult<T?>(value);
        }

        _logger.LogDebug("Cache miss: {Key}", key);
        return Task.FromResult<T?>(null);
    }

    public Task RemoveCache(string key, CancellationToken token = default)
    {
        _memoryCache.Remove(key);
        _logger.LogDebug("Removed from cache: {Key}", key);
        return Task.CompletedTask;
    }

    public Task SetCacheAsync<T>(string key, T value, CancellationToken token = default) where T : class
        => SetCacheAsync(key, value, _defaultMinutesToExpire, token);

    public Task SetCacheAsync<T>(string key, T value, int minutesToExpire, CancellationToken token = default) where T : class
    {
        var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutesToExpire),
            Size = json.Length
        };

        _memoryCache.Set(key, value, options);
        _logger.LogDebug("Stored in cache: {Key}", key);
        return Task.CompletedTask;
    }

    public async Task<T?> SetCacheIfEmptyAsync<T>(string key, Func<Task<T>> setter, CancellationToken token = default) where T : class
    {
        var cached = await GetCacheAsync<T>(key, token);
        if (cached != null)
            return cached;

        var value = await setter();

        var minutesToExpire = GetExpirationMinutesForType<T>();

        await SetCacheAsync(key, value, minutesToExpire, token);

        return value;
    }

    public async Task<T?> SetCacheIfEmptyAsync<T>(string key, Func<Task<T>> setter, int minutesToExpire, CancellationToken token = default) where T : class
    {
        var cached = await GetCacheAsync<T>(key, token);
        if (cached != null) return cached;

        var value = await setter();
        await SetCacheAsync(key, value, minutesToExpire, token);
        return value;
    }

    private int GetExpirationMinutesForType<T>()
    {
        var typeName = typeof(T).Name;
        if (_typeExpirations.TryGetValue(typeName, out int minutes))
            return minutes;

        return _defaultMinutesToExpire;
    }

}
