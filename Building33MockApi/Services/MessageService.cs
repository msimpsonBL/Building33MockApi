using Building33MockApi.Model;
using GatewayGrpcService.Protos;
using Grpc.Core;
using StackExchange.Redis;
using System.Text.Json;

namespace Building33MockApi.Services;

public class MessageService
{
    private readonly ICacheHandler _cacheHandler;
    private readonly ILogger<MessageService> _logger;

    public MessageService(ICacheHandler cacheHandler, ILogger<MessageService> logger)
    {
        _cacheHandler = cacheHandler;
        _logger = logger;
    }
    public async Task<bool> CreateStorageItemRequest(RsiPostMessage request)
    {
        try
        {
            var dataString = JsonSerializer.Serialize(request);
            await _cacheHandler.StringSetAsync(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.FFFFFFF"), dataString);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error storing message");
            return false;
        }
        return true;
    }
}
