using HotmartAPI.Hotmart.Models;
using HotmartAPI.Hotmart.Models.Purchases;
using HotmartAPI.Hotmart.Models.Subscriptions;
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
            var isSuccess = false;
            try
            {
                switch (hotmartUpdate.Event)
                {
                    case "PURCHASE_OUT_OF_SHOPPING_CART":
                        break;
                    case "PURCHASE_BILLET_PRINTED": // Aguardando pagamento
                    case "PURCHASE_APPROVED": // Compra aprovada
                        var purchaseBilletPrinted = hotmartUpdate.Data.ToObject<PurchaseBilletPrinted>();
                        _handleUpdateService.PurchaseBilletPrinted(purchaseBilletPrinted);
                        break;
                    case "PURCHASE_EXPIRED": // Compra expirada
                    case "PURCHASE_CHARGEBACK": // Chargeback
                    case "PURCHASE_REFUNDED": // Compra reembolsada
                    case "PURCHASE_CANCELED": // Compra cancelada
                    case "PURCHASE_COMPLETE": // Compra completa
                        var purchaseUpdated = hotmartUpdate.Data.ToObject<PurchaseUpdated>();
                        _handleUpdateService.PurchaseUpdated(purchaseUpdated);
                        break;
                    case "SWITCH_PLAN": // Assinatura => Troca de plano
                        var switchPlan = hotmartUpdate.Data.ToObject<SwitchPlan>();
                        _handleUpdateService.SwitchPlan(switchPlan);
                        break;
                    case "SUBSCRIPTION_CANCELLATION": // Assinatura => Cancelamento de Assinatura
                        var cancellation = hotmartUpdate.Data.ToObject<SubscriptionCancellation>();
                        _logger.LogInformation("SubscriptionCancellation: {0}", cancellation.Product.Name);
                        break;
                    case "PURCHASE_DELAYED": // Compra atrasada
                    case "PURCHASE_PROTEST": // Compra => Pedido de reembolso
                        // Ignore
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
