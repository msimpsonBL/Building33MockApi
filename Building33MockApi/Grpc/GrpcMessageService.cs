using GatewayGrpcService.Protos;
using Grpc.Core;
using StackExchange.Redis;

public class GrpcMessageService : GatewayGrpcMessagingService.GatewayGrpcMessagingServiceBase
{
    public override Task<RSIRecieved> CreateStorageItemRequest(RSIMessage request, ServerCallContext context)
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("REDIS"));
        IDatabase db = redis.GetDatabase();

        db.StringSet(DateTime.Now.ToString(), request.ToString());

        return Task.FromResult(new RSIRecieved { ItemIdentity = request.ItemIdentity });
    }
}
