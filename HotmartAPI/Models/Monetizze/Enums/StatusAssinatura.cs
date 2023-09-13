using System.Text.Json.Serialization;

namespace HotmartAPI.Models.Monetizze.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusAssinatura
    {
        AguardandoPagamento = 0,
        Ativa = 1,
        Cancelada = 2,
        Inadimplente = 3,
        Postado = 4,
    }
}
