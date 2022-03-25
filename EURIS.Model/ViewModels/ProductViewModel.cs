using System.Collections.Generic;

namespace EURIS.Model.ViewModels
{
    public sealed class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<CatalogViewModel> Catalogs { get; set; }

        public override string ToString()
        {
            string product = $"Product: ID: {Id}, Code: {Code}, Description: {Description}";
            return product;
        }
    }
}
