using System;
using System.Globalization;
using System.Text.Json.Serialization;
using HotmartAPI.Models.Monetizze.Enums;

namespace HotmartAPI.Models.Monetizze;

public class Venda
{    
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; }

    [JsonPropertyName("dataInicio")]
    public string DataInicio { get; set; }

    [JsonPropertyName("dataFinalizada")]
    public string DataFinalizada { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    public DateTime GetDataInicio()
    {
        return DateTime.ParseExact(DataInicio, format: "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public DateTime GetDataFinalizada()
    {
        return DateTime.ParseExact(DataFinalizada, format: "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public StatusVenda GetStatus()
    {
        return Status.ToLower() switch
        {
            "aguardando pagamento" => StatusVenda.AguardandoPagamento,
            "finalizada" => StatusVenda.Finalizada,
            "cancelada" => StatusVenda.Cancelada,
            "devolvida" => StatusVenda.Devolvida,
            "bloqueada" => StatusVenda.Bloqueada,
            "completa" => StatusVenda.Completa,
            _ => StatusVenda.AguardandoPagamento
        };
    }
}