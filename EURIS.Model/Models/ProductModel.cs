using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EURIS.Model.Models
{
    public sealed class ProductModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Code is a required field.")]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is a required field.")]
        [MaxLength(50)]
        public string Description { get; set; }

        [DisplayName("Catalogs")]
        public List<int> CatalogIds { get; set; }

        public override string ToString()
        {
            string product = $"Product: ID: {Id}, Code: {Code}, Description: {Description}, CatalogIds: {string.Join(", ", CatalogIds)}";
            return product;
        }
    }
}
