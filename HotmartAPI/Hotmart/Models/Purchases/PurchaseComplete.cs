using System.Text.Json.Serialization;

namespace HotmartAPI.Hotmart.Models.Purchases
{
    public class PurchaseComplete
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
