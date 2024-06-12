using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;

namespace Building33MockApi.Pages
{
    public class MessagesModel : PageModel
    {
        public List<KeyValuePair<DateTime, string>> Messages;

        public void OnGet()
        {
            Messages = new List<KeyValuePair<DateTime, string>>();
            var redisConnection = Environment.GetEnvironmentVariable("REDIS");

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnection);
            IDatabase db = redis.GetDatabase();

            var keys = redis.GetServer(redisConnection).Keys();

            foreach (var key in keys)
            {
                var value = db.StringGet(key);
                Messages.Add(new KeyValuePair<DateTime, string>(DateTime.Parse(key), value.ToString()));
            }
        }
    }
}
