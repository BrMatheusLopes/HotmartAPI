using HotmartAPI.Models.Hotmart;
using HotmartAPI.Models.Hotmart.Purchases;
using HotmartAPI.Models.Hotmart.Subscriptions;
using HotmartAPI.Services;
using HotmartAPI.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HotmartAPI.Controllers
{
    public class HotmartController : Controller
    {
        private readonly HandleUpdateService _handleUpdateService;
        private readonly ILogger<HotmartController> _logger;

        public HotmartController(HandleUpdateService handleUpdateService, ILogger<HotmartController> logger)
        {
            _logger = logger;
            _handleUpdateService = handleUpdateService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] HotmartUpdate hotmartUpdate)
        {
            var HasX_HOTMART_HOTTOK = Request.Headers.TryGetValue("X-HOTMART-HOTTOK", out var hottok);
            if (!HasX_HOTMART_HOTTOK || !hottok.Equals(Startup.HotmartConfig.Hottok) || hotmartUpdate == null)
            {
                _logger.LogInformation("HasX_HOTMART_HOTTOK: {0}, Hottok value: {1}", HasX_HOTMART_HOTTOK, hottok);
                return Unauthorized();
            }

            try
            {
                switch (hotmartUpdate.Event)
                {
                    case "PURCHASE_OUT_OF_SHOPPING_CART": // Abandono de carrinho
                    case "PURCHASE_DELAYED": // Compra atrasada
                    case "PURCHASE_PROTEST": // Compra => Pedido de reembolso
                        break;
                    //case "PURCHASE_BILLET_PRINTED": // Aguardando pagamento
                    case "PURCHASE_APPROVED": // Compra aprovada
                        var purchaseApproved = hotmartUpdate.Data.ToObject<PurchaseApproved>();
                        _handleUpdateService.PurchaseApproved(purchaseApproved);
                        break;
                    case "PURCHASE_CHARGEBACK": // Chargeback
                    case "PURCHASE_REFUNDED": // Compra reembolsada
                    case "PURCHASE_CANCELED": // Compra cancelada
                    case "PURCHASE_COMPLETE": // Compra completa
                    case "PURCHASE_EXPIRED": // Compra expirada
                        var purchaseUpdated = hotmartUpdate.Data.ToObject<PurchaseUpdated>();
                        _handleUpdateService.PurchaseUpdated(purchaseUpdated, hotmartUpdate.Event == "PURCHASE_COMPLETE");
                        break;
                    case "SWITCH_PLAN": // Assinatura => Troca de plano
                        var switchPlan = hotmartUpdate.Data.ToObject<SwitchPlan>();
                        _handleUpdateService.SwitchPlan(switchPlan);
                        break;
                    case "SUBSCRIPTION_CANCELLATION": // Assinatura => Cancelamento de Assinatura
                        var subscriptionCancellation = hotmartUpdate.Data.ToObject<SubscriptionCancellation>();
                        _handleUpdateService.SubscriptionCancellation(subscriptionCancellation);
                        break;
                }
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("api/hotmart")]
        public IActionResult GetAllOrders([FromQuery(Name = "password")] string password)
        {
            if (password != "heroku_hotmart_orders")
                return Unauthorized();

            return Ok(_handleUpdateService.GetAllOrders());
        }
    }
}
