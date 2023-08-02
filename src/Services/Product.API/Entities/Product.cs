using Contracts.Domains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace Product.API.Entities
{
    public class CatalogProduct : EntityAuditBase<long>
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string No { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Sumary { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        // decimal(12.2)
        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

    }
}
