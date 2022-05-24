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
        private const string Token = "3710A7F08220611D3C147180358DF75BC40CC1D94C8F5F0263C4EBAE5C4F6FAD";
        public LicenseController(IOrderRepository repository, ILogger<LicenseController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /*        [HttpPost]
                public IActionResult Post([FromBody] LicenseResponse license)
                {
                    var HasToken = Request.Headers.TryGetValue("X-TOKEN", out var token);
                    if (!HasToken || token != Token)
                        return Unauthorized(new License { Msg = "Licença inválida!" }.SerializeToJson());

                    if (license == null || string.IsNullOrEmpty(license.Email) || string.IsNullOrEmpty(license.Transaction))
                        return Unauthorized(new License { Msg = "Licença inválida!" }.SerializeToJson());

                    var order = _repository.GetOrderByEmailAndTransaction(license.Email, license.Transaction);
                    if (order == null)
                        return NotFound(new License { Msg = "Licença não encontrada!"}.SerializeToJson());

                    return Ok(new License
                    {
                        BuyerEmail = order.BuyerEmail,
                        Status = order.Status,
                        ExpirationDate = order.ExpirationDate,
                        SubscriptionPlanName = order.SubscriptionPlanName,
                        SubscriptionStatus = order.SubscriptionStatus
                    });
                }*/

        [HttpPost]
        public IActionResult Post([FromBody] LicenseResponse license)
        {
            var HasToken = Request.Headers.TryGetValue("X-TOKEN", out var token);
            if (!HasToken || token != Token)
                return Unauthorized(ReturnErrorMessage("Licença inválida!"));

            if (license == null || string.IsNullOrEmpty(license.Email) || string.IsNullOrEmpty(license.Transaction))
                return Unauthorized(ReturnErrorMessage("Licença inválida!"));

            var order = _repository.GetOrderByEmailAndTransaction(license.Email, license.Transaction);
            if (order == null)
                return NotFound(ReturnErrorMessage("Licença não encontrada!"));

            return Ok(ReturnOKMessage(order));
        }

        private string ReturnErrorMessage(string message)
        {
            return CryptoHelper.Encrypt(new License { Msg = message }.SerializeToJson(), Constants.Password);
        }

        private string ReturnOKMessage(Order order)
        {
            return CryptoHelper.Encrypt(new License
            {
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
