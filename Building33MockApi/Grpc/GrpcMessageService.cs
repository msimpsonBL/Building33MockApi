using GatewayGrpcService.Protos;
using Grpc.Core;
using StackExchange.Redis;

public class GrpcMessageService : GatewayGrpcMessagingService.GatewayGrpcMessagingServiceBase
{
    private readonly IDatabase _db;

    public GrpcMessageService(IDatabase db)
    {
        _db = db;
    }
    
    public override Task<RSIRecieved> CreateStorageItemRequest(RSIMessage request, ServerCallContext context)
    {
        _db.StringSet(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.FFFFFFF"), request.ToString());

        return Task.FromResult(new RSIRecieved { ItemIdentity = request.ItemIdentity });
    }
}
