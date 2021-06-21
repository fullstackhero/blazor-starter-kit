using BlazorHero.CleanArchitecture.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Catalog
{
    public class Product : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }

        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }

        public string Description { get; set; }
        public decimal Rate { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
    }
}