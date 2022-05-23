using Microsoft.AspNetCore.Mvc;

namespace HotmartAPI.Controllers
{
    [ApiController]
    [Route("api/license/validation")]
    public class LicenseController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromQuery(Name = "email")] string idTransacao, [FromQuery(Name = "email")] string email)
        {
            return Ok($"Email: {email}, Id da Transação: {idTransacao}");
        }
    }
}
