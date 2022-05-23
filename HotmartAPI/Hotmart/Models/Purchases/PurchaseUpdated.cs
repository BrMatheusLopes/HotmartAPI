using System.Text.Json.Serialization;

namespace HotmartAPI.Hotmart.Models.Purchases
{
    // PURCHASE_EXPIRED, PURCHASE_CHARGEBACK, PURCHASE_REFUNDED, PURCHASE_CANCELED, PurchaseComplete
    public class PurchaseUpdated
    {
        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("buyer")]
        public Buyer Buyer { get; set; }

        [JsonPropertyName("producer")]
        public Producer Producer { get; set; }

        [JsonPropertyName("purchase")]
        public Purchase Purchase { get; set; }
    }
}
