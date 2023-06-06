using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Manager.Models
{
    public class Product:AuditFields
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;
            set;
        }
        public string ProductName { get; set; }

        public string ManufrecturerName { get; set; }

        public string ProductType { get; set; }
        public int CategoryId { get; set; }

        public int CostPrice { get; set; }

        public int RetailPrice { get; set; }

        public bool Status { get; set; }

        public DateTime ManufrecturingDate  { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
