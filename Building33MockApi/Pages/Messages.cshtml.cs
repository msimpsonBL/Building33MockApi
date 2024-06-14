using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using System.Net;

namespace Building33MockApi.Pages
{
    public class MessagesModel : PageModel
    {
        private readonly IDatabase _cache;
        public List<KeyValuePair<DateTime, string>> Messages;

        public MessagesModel(IDatabase cache) {
            _cache = cache;
        }

        public void OnGet()
        {
            Messages = new List<KeyValuePair<DateTime, string>>();
            //var redisConnection = Environment.GetEnvironmentVariable("REDIS");

            //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnection);
            //IDatabase db = redis.GetDatabase();
            EndPoint endPoint = _cache.Multiplexer.GetEndPoints().First();
            var keys = _cache.Multiplexer.GetServer(endPoint).Keys();

            foreach (var key in keys)
            {
                var value = _cache.StringGet(key);
                Messages.Add(new KeyValuePair<DateTime, string>(DateTime.Parse(key), value.ToString()));
            }
        }

        public IActionResult OnPost()
        {
            EndPoint endPoint = _cache.Multiplexer.GetEndPoints().First();
            _cache.Multiplexer.GetServer(endPoint).FlushDatabase();
            return RedirectToPage("/messages");
        }
    }
}
