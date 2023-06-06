namespace Product_Manager.Models
{
    public class AuditFields
    {
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
