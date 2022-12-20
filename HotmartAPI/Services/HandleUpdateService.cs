using HotmartAPI.Data.Entities;
using HotmartAPI.Hotmart.Models.Purchases;
using HotmartAPI.Hotmart.Models.Subscriptions;
using HotmartAPI.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public IEnumerable<Order> GetAllOrders()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Pagamento aprovado
        /// </summary>
        /// <param name="purchaseApproved"></param>
        public void PurchaseApproved(PurchaseApproved purchaseApproved)
        {
            var newOrder = new Data.Entities.Order
            {
                BuyerEmail = purchaseApproved.Buyer.Email,
                Status = purchaseApproved.Purchase.Status,
                Transaction = purchaseApproved.Purchase.Transaction,
                ExpirationDate = GetExpirationTimeByPlan(purchaseApproved.Subscription.Plan.Name),
                SubscriberCode = purchaseApproved.Subscription.Subscriber.Code,
                SubscriptionPlanName = purchaseApproved.Subscription.Plan.Name.Trim(),
                SubscriptionStatus = purchaseApproved?.Subscription.Status
            };

            // Verifica se o pedido já existe no banco.
            var order = _repository.GetOrderByEmailAndCode(purchaseApproved.Buyer.Email, purchaseApproved.Subscription.Subscriber.Code);
            if (order == null)
            {
                var createdOrder = _repository.Create(newOrder);
                _logger.LogInformation("Pedido {0} criado, status: {1}", newOrder.Transaction, newOrder.Status);
            }
            else
            {
                newOrder.Id = order.Id;
                var updatedOrder = _repository.Update(newOrder);
                _logger.LogInformation("Pedido {0} modificado, status: {1}", order.Transaction, order.Status);
            }
        }

        private DateTime GetExpirationTimeByPlan(string planName)
        {
            var timeNow = DateTime.UtcNow;
            switch (planName.ToLower().Trim())
            {
                case "plano mensal":
                    timeNow = timeNow.AddMonths(1);
                    break;
                case "plano trimestral":
                    timeNow = timeNow.AddMonths(3);
                    break;
                default:
                    _logger.LogError("Plano >{0}< não encontrado", planName);
                    break;
            }

            return timeNow;
        }

        /// <summary>
        /// Status da compra atualizado
        /// </summary>
        /// <param name="purchaseUpdated">dados do pedido atualizado</param>
        public void PurchaseUpdated(PurchaseUpdated purchaseUpdated, bool isPurchaseComplete)
        {
            var order = _repository.GetOrderByEmailAndTransaction(purchaseUpdated.Buyer.Email, purchaseUpdated.Purchase.Transaction);
            if (order == null)
                return;

            order.Status = purchaseUpdated.Purchase.Status;
            if (!isPurchaseComplete)
                order.SubscriptionStatus = "INACTIVE";
            _repository.Update(order);
            _logger.LogInformation("Status do pedido: {0}, alterado para: {1}", order.Transaction, order.Status);
        }

        #region Troca de Plano e Cancelamento de Assinatura

        /// <summary>
        /// Troca de plano
        /// </summary>
        /// <param name="SwitchPlan"></param>
        public void SwitchPlan(SwitchPlan SwitchPlan)
        {
            var order = _repository.GetOrderByEmailAndCode(SwitchPlan.Subscription.User.Email, SwitchPlan.Subscription.SubscriberCode);
            if (order == null)
            {
                _logger.LogError("(SwitchPlan) Pedido de email: {0}, e código do assinante: {1} não encontrados", SwitchPlan.Subscription.User.Email, SwitchPlan.Subscription.SubscriberCode);
                return;
            }

            var plano = SwitchPlan.Plans.FirstOrDefault(x => x.Current == true);
            var plano2 = SwitchPlan.Plans.FirstOrDefault(x => x.Current == false);

            order.SubscriberCode = SwitchPlan.Subscription.SubscriberCode;
            order.SubscriptionPlanName = plano?.Name;
            order.SubscriptionStatus = SwitchPlan.Subscription.Status;

            _repository.Update(order);
            _logger.LogInformation("(SwitchPlan) Plano do pedido: {0}, alterado de {1} para {2}", order.Transaction, plano?.Name, plano2?.Name);
        }

        /// <summary>
        /// Cancelamento de Assinatura
        /// </summary>
        /// <param name="subscriptionCancellation"></param>
        public void SubscriptionCancellation(SubscriptionCancellation cancellation)
        {
            var order = _repository.GetOrderByEmailAndCode(cancellation.Subscriber.Email, cancellation.Subscriber.Code);
            if (order == null)
            {
                _logger.LogError("(SubscriptionCancellation) Pedido de email: {0}, e código do assinante: {1} não encontrados", cancellation.Subscriber.Email, cancellation.Subscriber.Code);
                return;
            }

            order.Status = "CANCELLED";
            order.SubscriptionStatus = "INACTIVE";
            _repository.Update(order);
            _logger.LogInformation("(SubscriptionCancellation) Assinatura do pedido: {0} cancelada!", order.Transaction);
        }

        #endregion
    }
}
