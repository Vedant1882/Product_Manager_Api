using Product_Manager.Models;

namespace Product_Manager.ViewModel
{
    public class CategoryWithPage
    {
        public List<Category> data { get; set; }
        public int totalPages { get; set; }
    }
}
