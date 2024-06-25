using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Building33MockApi.Pages
{
    public class MessagesModel : PageModel
    {
        private readonly ICacheHandler _cacheHandler;
        public readonly ILogger<MessagesModel> _logger;

        public List<KeyValuePair<string, string>> Messages { get; set; }

        public MessagesModel(ICacheHandler cacheHandler, ILogger<MessagesModel> logger)
        {
            _cacheHandler = cacheHandler;
            _logger = logger;
        }

        public void OnGet()
        {
            Messages = new List<KeyValuePair<string, string>>();

            var keys = _cacheHandler.StringGetAll();

            foreach (var key in keys)
            {
                var value = _cacheHandler.StringGetAsync(key).Result;
                Messages.Add(new KeyValuePair<string, string>(key.ToString(), value.ToString()));
            }
        }

        public IActionResult OnPost()
        {
            _cacheHandler.FlushAll();
            return RedirectToPage("/messages");
        }
    }
}
