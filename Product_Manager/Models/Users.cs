using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Manager.Models
{
    public class AppUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;
            set;
        }
        [Required]
        public string FirstName
        {
            get;
            set;
        }
        public string? LastName
        {
            get;
            set;
        }
        [Required]
        public string Email
        {
            get;
            set;
        }
        public byte[] Password
        {
            get;
            set;
        }
        public byte[] Key
        {
            get;
            set;
        }
        public long? PhoneNumber
        {
            get;
            set;
        }
        public string? Address
        {
            get;
            set;
        }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CreatedById
        {
            get;
            set;
        } = 1;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UpdatedById
        {
            get;
            set;
        } = 1;
        public DateTime? DeletedAt { get; set; }

        public int DeletedById
        {
            get;
            set;
        } 
    }
}
