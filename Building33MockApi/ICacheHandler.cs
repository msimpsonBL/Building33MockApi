using StackExchange.Redis;

namespace Building33MockApi;

public interface ICacheHandler : IDisposable
{
    Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null);
    Task<string?> StringGetAsync(string key);
    Task StringDeleteAsync(string key);
    Task<bool> StringExistsAsync(string key);
    IEnumerable<RedisKey> StringGetAll();
}
