using StackExchange.Redis;

namespace Building33MockApi;

public interface IRedisDbProvider : IDisposable
{
    public IDatabase database { get; }
    public IServer server { get; }
}
