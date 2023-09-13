using System;
using System.Globalization;
using System.Text.Json.Serialization;
using HotmartAPI.Models.Monetizze.Enums;

namespace HotmartAPI.Models.Monetizze
{
    public class MonetizzePostback
    {
        [JsonPropertyName("chave_unica")]
        public string ChaveUnica { get; set; }

        [JsonPropertyName("codigo_venda")]
        public string CodigoVenda { get; set; }

        [JsonPropertyName("codigo_status")]
        public string CodigoStatus { get; set; }


        [JsonPropertyName("produto")]
        public Produto Produto { get; set; }

        [JsonPropertyName("tipoPostback")]
        public TipoPostback TipoPostback { get; set; }

        [JsonPropertyName("venda")]
        public Venda Venda { get; set; }

        [JsonPropertyName("plano")]
        public Plano Plano { get; set; }

        [JsonPropertyName("comprador")]
        public Comprador Comprador { get; set; }

        [JsonPropertyName("assinatura")]
        public Assinatura Assinatura { get; set; }

        public bool TryParseAssinatura(out Assinatura assinatura)
        {
            assinatura = Assinatura;
            return assinatura != null;
        }
    }

    public class Assinatura
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data_assinatura")]
        public string DataAssinatura { get; set; }

        public StatusAssinatura GetStatus()
        {
            return Status.ToLower() switch
            {
                "aguardando pagamento" => StatusAssinatura.AguardandoPagamento,
                "ativa" => StatusAssinatura.Ativa,
                "cancelada" => StatusAssinatura.Cancelada,
                "inadimplente" => StatusAssinatura.Inadimplente,
                "postado" => StatusAssinatura.Postado,
                _ => StatusAssinatura.AguardandoPagamento
            };
        }

        /// <summary>
        /// Eu ainda não sei se essa é a data de expiração ou a data inicial do plano
        /// </summary>
        /// <returns></returns>
        public DateTime GetDataAssinatura()
        {
            return DateTime.ParseExact(DataAssinatura, format: "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }

    public class Plano
    {
        [JsonPropertyName("referencia")]
        public string Referencia { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }

    public class Produto
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("chave")]
        public string Chave { get; set; }
    }

    public class TipoPostback
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }
    }
}
