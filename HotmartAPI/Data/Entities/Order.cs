using HotmartAPI.Data.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotmartAPI.Data.Entities
{
    public class Order : BaseEntity
    {
        /// <summary>
        /// Email do comprador
        /// </summary>
        [MaxLength(128), Required]
        public string BuyerEmail { get; set; }

        /// <summary>
        /// Status da compra => BILLET_PRINTED, COMPLETED, APPROVED
        /// </summary>
        [MaxLength(32)]
        public string Status { get; set; }

        /// <summary>
        /// ID da transação
        /// </summary>
        [MaxLength(32)]
        public string Transaction { get; set; }

        /// <summary>
        /// Data de expiração da licença (estimativa)
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Código exclusivo de um assinante.
        /// </summary>
        [MaxLength(32), Required]
        public string SubscriberCode { get; set; }

        /// <summary>
        /// Nome do plano adquirido. Enviado apenas em caso de venda de assinaturas.
        /// </summary>
        [MaxLength(64)]
        public string SubscriptionPlanName { get; set; }

        /// <summary>
        /// Mostra os status da assinatura: ACTIVE, INACTIVE, DELAYED, CANCELLED_BY_CUSTOMER, CANCELLED_BY_SELLER, CANCELLED_BY_ADMIN, STARTED ou OVERDUE.
        /// </summary>
        [MaxLength(32)]
        public string SubscriptionStatus { get; set; }
    }
}
