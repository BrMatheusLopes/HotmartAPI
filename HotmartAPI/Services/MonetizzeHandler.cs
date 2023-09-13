using System;
using HotmartAPI.Data.Entities;
using HotmartAPI.Models.Hotmart.Purchases;
using HotmartAPI.Models.Monetizze;
using HotmartAPI.Repository;
using Microsoft.Extensions.Logging;

namespace HotmartAPI.Services
{
    public class MonetizzeHandler
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<MonetizzeHandler> _logger;

        public MonetizzeHandler(IOrderRepository repository, ILogger<MonetizzeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void CompraAprovada(MonetizzePostback postback)
        {
            if (!postback.TryParseAssinatura(out var assinatura))
                return;

            var newOrder = new Order
            {
                BuyerEmail = postback.Comprador.Email,
                Status = postback.CodigoStatus,
                Transaction = postback.CodigoVenda.Length > 32 ? postback.CodigoVenda.Substring(0, 32) : postback.CodigoVenda,
                ExpirationDate = DateTime.Now.AddYears(1),
                SubscriberCode = $"M_{postback.Comprador.CnpjCpf}",
                SubscriptionPlanName = postback.Plano.Nome,
                SubscriptionStatus = postback.Assinatura.Status
            };

            // Verifica se o pedido já existe no banco.
            var order = _repository.GetOrderByEmailAndCode(postback.Comprador.Email, $"M_{postback.Comprador.CnpjCpf}");
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
    }
}
