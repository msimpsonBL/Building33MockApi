using Building33MockApi.Model;
using Building33MockApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Building33MockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;
        private readonly ILogger<MessageController> _logger;

        public MessageController(MessageService messageService, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        // POST api/<MessageController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RsiPostMessage value)
        {
            try
            {
                await _messageService.CreateStorageItemRequest(value);
             }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating storage item request");
            }
            var returnResult = new RsiReceivedMessage { ItemIdentity = value.Identifier };
            return Ok(returnResult);
        }

        [HttpGet]
        public IActionResult Get()
        { 
            return Ok();
        }
    }
}
