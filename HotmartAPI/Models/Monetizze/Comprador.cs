using System.Text.Json.Serialization;

namespace HotmartAPI.Models.Monetizze;

public class Comprador
{
    [JsonPropertyName("nome")]
    public string Nome { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("cnpj_cpf")]
    public string CnpjCpf { get; set; }
}