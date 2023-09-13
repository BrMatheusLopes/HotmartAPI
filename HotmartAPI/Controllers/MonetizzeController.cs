using System;
using System.Text.Json;
using System.Threading.Tasks;
using HotmartAPI.Models.Monetizze;
using HotmartAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HotmartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonetizzeController : ControllerBase
    {
        private readonly MonetizzeHandler _monetizzeHandler;
        private readonly PersonalBotService _personalBot;
        private readonly ILogger<MonetizzeController> _logger;

        public MonetizzeController( MonetizzeHandler monetizzeHandler, PersonalBotService personalBot, ILogger<MonetizzeController> logger)
        {
            _monetizzeHandler = monetizzeHandler;
            _personalBot = personalBot;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] object postback)
        {
            _ = TrySendMessage(postback);

            try
            {
                var obj = JsonSerializer.Deserialize<MonetizzePostback>(postback.ToString()!);
                if (obj != null)
                {

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MonetizzeController: {ex.Message}");
            }

            return Ok();
        }

        private async Task TrySendMessage(object postback)
        {
            try
            {
                await _personalBot.SendDocumentAsync(postback.ToString(), "MonetizzePostBack.json");
            }
            catch (Exception e)
            {
                _logger.LogError($"TrySendMessage: {e.Message}");
            }
        }
    }
}
