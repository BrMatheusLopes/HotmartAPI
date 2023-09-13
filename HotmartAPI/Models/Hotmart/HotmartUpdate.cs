using System.Text.Json.Serialization;

namespace HotmartAPI.Models.Hotmart
{
    public class HotmartUpdate
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("creation_date")]
        public long CreationDate { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}
