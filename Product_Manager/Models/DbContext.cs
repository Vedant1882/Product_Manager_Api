using Microsoft.EntityFrameworkCore;

namespace Product_Manager.Models
{
    public class UserConext:DbContext
    {
        public UserConext(DbContextOptions options) : base(options) { }
        public DbSet<AppUsers> AppUsers
        {
            get;
            set;
        }
        public DbSet<Category> Categories
        {
            get;
            set;
        }
        public DbSet<Product> Products
        {
            get;
            set;
        }
    }
}
