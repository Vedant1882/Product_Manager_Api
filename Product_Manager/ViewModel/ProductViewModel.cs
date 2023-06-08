using System.Reflection.Metadata;

namespace Product_Manager.ViewModel
{
    public class ProductViewModel
    {

        public int Id { get; set; }
        public string ProductName { get; set; }

        public string ManufrecturerName { get; set; }

        public string ProductType { get; set; }
        public int CategoryId { get; set; }

        public int CostPrice { get; set; }

        public int RetailPrice { get; set; }

        public bool Status { get; set; }

        public DateTime ManufrecturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string ImageUrl { get; set; }
        public string? Name { get; set; }
    }
}
