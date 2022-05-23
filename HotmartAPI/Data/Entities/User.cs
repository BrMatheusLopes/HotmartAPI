using HotmartAPI.Data.Entities.Base;

namespace HotmartAPI.Data.Entities
{
    public class User : BaseEntity
    {
        /// <summary>
        /// Nome do comprador
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// E-mail do comprador.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Código exclusivo de um assinante.
        /// </summary>
        public string Code { get; set; }
    }
}
