using System;
using HotmartAPI.Data.Entities;
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
                Transaction = postback.CodigoVenda,
                ExpirationDate = DateTime.Now.AddYears(1),
                SubscriberCode = $"M_{postback.Comprador.CnpjCpf}",
                SubscriptionPlanName = postback.Plano.Nome,
                SubscriptionStatus = postback.Assinatura.Status
            };
        }
    }
}
