using Product_Manager.Models;
using Product_Manager.ViewModel;

namespace Product_Manager.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategory();
        public Task<Category> GetCategoryById(int id);

        public Task<ResponseModel> SaveCategory(Category CategoryModel);

        public Task<ResponseModel> DeleteCategory(int id);
    }
}
