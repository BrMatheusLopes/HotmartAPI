using System.Text.Json.Serialization;

namespace HotmartAPI.Models.Hotmart.Subscriptions
{
    public class SubscriptionCancellation
    {
        [JsonPropertyName("date_next_charge")]
        public long DateNextCharge { get; set; }

        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("actual_recurrence_value")]
        public double ActualRecurrenceValue { get; set; }

        [JsonPropertyName("subscriber")]
        public Subscriber Subscriber { get; set; }

        [JsonPropertyName("subscription")]
        public Subscription Subscription { get; set; }

        [JsonPropertyName("cancellation_date")]
        public long CancellationDate { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class Subscriber
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class Subscription
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("subscriber_code")]
        public string SubscriberCode { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("plan")]
        public Plan Plan { get; set; }

        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; }
    }

    public class Plan
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("current")]
        public bool Current { get; set; }
    }

    public class User
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
