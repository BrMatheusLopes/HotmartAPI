using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotmartAPI.Data.Entities.Base
{
    public class BaseEntity
    {
        [Column("id")]
        [Key]
        public long Id { get; set; }
    }
}
