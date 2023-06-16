using Product_Manager.Models;

namespace Product_Manager.ViewModel
{
    public class UsersWithPage
    {
        public List<AppUsers> data { get; set; }
        public int totalPages { get; set; }
    }
}
