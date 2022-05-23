using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HotmartAPI.Hotmart.Models.Purchases
{
    public class PurchaseBilletPrinted
    {
        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("buyer")]
        public Buyer Buyer { get; set; }

        [JsonPropertyName("producer")]
        public Producer Producer { get; set; }

        [JsonPropertyName("purchase")]
        public Purchase Purchase { get; set; }

        [JsonPropertyName("subscription")]
        public Subscription Subscription { get; set; }
    }

    public class Payment
    {
        [JsonPropertyName("billet_url")]
        public string BilletUrl { get; set; }

        [JsonPropertyName("billet_barcode")]
        public string BilletBarcode { get; set; }

        [JsonPropertyName("installments_number")]
        public int InstallmentsNumber { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Plan
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Price
    {
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }

    public class Producer
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("has_co_production")]
        public bool HasCoProduction { get; set; }

    }

    public class Purchase
    {
        [JsonPropertyName("order_date")]
        public long OrderDate { get; set; }

        [JsonPropertyName("price")]
        public Price Price { get; set; }

        [JsonPropertyName("payment")]
        public Payment Payment { get; set; }

        [JsonPropertyName("transaction")]
        public string Transaction { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class Subscriber
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

    public class Subscription
    {
        [JsonPropertyName("subscriber")]
        public Subscriber Subscriber { get; set; }

        [JsonPropertyName("plan")]
        public Plan Plan { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class Buyer
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
