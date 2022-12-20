using HotmartAPI.Data.Entities;
using HotmartAPI.Helpers;
using HotmartAPI.Repository;
using HotmartAPI.Utils;
using HotmartAPI.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HotmartAPI.Controllers
{
    [ApiController]
    [Route("api/license/validation")]
    public class LicenseController : Controller
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<LicenseController> _logger;

        public LicenseController(IOrderRepository repository, ILogger<LicenseController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LicenseResponse license)
        {
            var HasToken = Request.Headers.TryGetValue("X-TOKEN", out var token);
            if (!HasToken || token != Constants.Token)
                return Unauthorized(ReturnErrorMessage("Licença inválida!"));

            if (license == null || string.IsNullOrEmpty(license.Email) || string.IsNullOrEmpty(license.Transaction))
                return Unauthorized(ReturnErrorMessage("Licença inválida!"));

            var order = _repository.GetOrderByEmailAndTransaction(license.Email, license.Transaction);
            if (order == null)
                return NotFound(ReturnErrorMessage("Licença não encontrada!"));

            string msg = "Sua licença expirou!";
            if (order.SubscriptionStatus == "ACTIVE")
            {                
                // Verifica se a licença expirou
                var timeNow = DateTime.Now;
                if (timeNow > order.ExpirationDate)
                {
                    order.SubscriptionStatus = "EXPIRED";
                }
            }

            return Ok(ReturnOKMessage(order, msg));
        }

        private string ReturnErrorMessage(string message)
        {
            return CryptoHelper.Encrypt(new License { Msg = message }.SerializeToJson(), Constants.Password);
        }

        private string ReturnOKMessage(Order order, string message = null)
        {
            return CryptoHelper.Encrypt(new License
            {
                Msg = message,
                BuyerEmail = order.BuyerEmail,
                Status = order.Status,
                ExpirationDate = order.ExpirationDate,
                SubscriptionPlanName = order.SubscriptionPlanName,
                SubscriptionStatus = order.SubscriptionStatus
            }.SerializeToJson(), Constants.Password);
        }
    }

    public class License : ErrorMessage
    {
        public string BuyerEmail { get; set; }
        public string Status { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SubscriptionPlanName { get; set; }
        public string SubscriptionStatus { get; set; }
    }

    public class ErrorMessage
    {
        public string Msg { get; set; }
    }

    public class LicenseResponse
    {
        public string Email { get; set; }
        public string Transaction { get; set; }
    }
}
