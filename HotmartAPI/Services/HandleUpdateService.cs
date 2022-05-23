using HotmartAPI.Hotmart.Models.Purchases;
using HotmartAPI.Hotmart.Models.Subscriptions;
using HotmartAPI.Repository;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace HotmartAPI.Services
{
    public class HandleUpdateService
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<HandleUpdateService> _logger;

        public HandleUpdateService(IOrderRepository repository, ILogger<HandleUpdateService> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        public void PurchaseBilletPrinted(PurchaseBilletPrinted purchaseBilletPrinted)
        {
            var newOrder = new Data.Entities.Order
            {
                BuyerEmail = purchaseBilletPrinted.Buyer.Email,
                Status = purchaseBilletPrinted.Purchase.Status,
                Transaction = purchaseBilletPrinted.Purchase.Transaction,
                ExpirationDate = default,
                SubscriberCode = purchaseBilletPrinted.Subscription.Subscriber.Code,
                SubscriptionPlanName = purchaseBilletPrinted.Subscription.Plan.Name,
                SubscriptionStatus = purchaseBilletPrinted?.Subscription.Status
            };

            // Verifica se o pedido já existe no banco de dados e atualiza
            var order = _repository.GetOrderByEmailAndTransaction(purchaseBilletPrinted.Buyer.Email, purchaseBilletPrinted.Purchase.Transaction);
            if (order != null)
            {
                var updatedOrder = _repository.Update(newOrder);
                return;
            }

            var createdOrder = _repository.Create(newOrder);
            _logger.LogInformation("Pedido {0} criado, status: {1}", order.Transaction, order.Status);
        }

        /// <summary>
        /// Status da compra atualizado
        /// </summary>
        /// <param name="purchaseUpdated">dados do pedido atualizado</param>
        public void PurchaseUpdated(PurchaseUpdated purchaseUpdated)
        {
            var order = _repository.GetOrderByEmailAndCode(purchaseUpdated.Buyer.Email, purchaseUpdated.Purchase.Transaction);
            if (order == null)
                return;

            order.Status = purchaseUpdated.Purchase.Status;
            order.SubscriptionStatus = "INACTIVE";
            _repository.Update(order);
            _logger.LogInformation("Status do pedido: {0}, alterado para {1}", order.Transaction, order.Status);
        }

        /// <summary>
        /// Compra reembolsada
        /// </summary>
        /// <param name="purchaseRefunded"></param>
        public void PurchaseRefunded(PurchaseRefunded purchaseRefunded)
        {
            var order = _repository.GetOrderByEmailAndTransaction(purchaseRefunded.Buyer.Email, purchaseRefunded.Purchase.Transaction);
            if (order == null)
                return;

            order.Status = purchaseRefunded.Purchase.Status;
            order.SubscriptionStatus = "INACTIVE";
            _repository.Update(order);
            _logger.LogInformation("Status do pedido: {0}, alterado para {1}", order.Transaction, order.Status);
        }

        /// <summary>
        /// Troca de plano
        /// </summary>
        /// <param name="SwitchPlan"></param>
        public void SwitchPlan(SwitchPlan SwitchPlan)
        {
            var order = _repository.GetOrderByEmailAndCode(SwitchPlan.Subscription.User.Email, SwitchPlan.Subscription.SubscriberCode);
            if (order == null)
            {
                _logger.LogError("(SwitchPlan) Pedido de email {0} e código do assinante {1} não encontrados", SwitchPlan.Subscription.User.Email, SwitchPlan.Subscription.SubscriberCode);
                return;
            }

            var plano = SwitchPlan.Plans.FirstOrDefault(x => x.Current == true);
            var plano2 = SwitchPlan.Plans.FirstOrDefault(x => x.Current == false);

            order.SubscriberCode = SwitchPlan.Subscription.SubscriberCode;
            order.SubscriptionPlanName = plano?.Name;
            order.SubscriptionStatus = SwitchPlan.Subscription.Status;

            _repository.Update(order);
            _logger.LogInformation("(SwitchPlan) plano do pedido: {0}, alterado de {1} para {2}", order.Transaction, plano?.Name, plano2?.Name);
        }
    }
}
