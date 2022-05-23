using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HotmartAPI.Hotmart.Models.Subscriptions
{
    public class SwitchPlan
    {
        [JsonPropertyName("switch_plan_date")]
        public long SwitchPlanDate { get; set; }

        [JsonPropertyName("subscription")]
        public Subscription Subscription { get; set; }

        [JsonPropertyName("plans")]
        public List<Plan> Plans { get; set; }
    }
}
