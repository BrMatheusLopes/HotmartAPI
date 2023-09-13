using System.Text.Json.Serialization;

namespace HotmartAPI.Models.Monetizze.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusVenda
    {
        AguardandoPagamento = 1,
        Finalizada = 2,
        Cancelada = 3,
        Devolvida = 4,
        Bloqueada = 5,
        Completa = 6
    }
}