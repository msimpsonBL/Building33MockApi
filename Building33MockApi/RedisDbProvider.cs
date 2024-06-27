using StackExchange.Redis;

namespace Building33MockApi;

public class RedisDbProvider : IRedisDbProvider
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
    private bool disposed = false;
    private readonly string _connectionString;

    public RedisDbProvider(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(connectionString);
        var _muxer = ConnectionMultiplexer.Connect(_connectionString);
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() => _muxer);
        server = _muxer.GetServer(_muxer.GetEndPoints().Single());
    }

    public IDatabase database => _lazyConnection.Value.GetDatabase();
    public IServer server { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            if (_lazyConnection.IsValueCreated)
            {
                _lazyConnection.Value.Dispose();
            }
        }

        disposed = true;
    }
}
