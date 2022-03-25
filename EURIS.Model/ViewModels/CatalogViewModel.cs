using System.Collections.Generic;

namespace EURIS.Model.ViewModels
{
    public sealed class CatalogViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<ProductViewModel> Products { get; set; }

        public override string ToString()
        {
            string product = $"Catalog: ID: {Id}, Code: {Code}, Description: {Description}";
            return product;
        }
    }
}
