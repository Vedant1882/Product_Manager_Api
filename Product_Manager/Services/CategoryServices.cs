using Microsoft.EntityFrameworkCore;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.ViewModel;

namespace Product_Manager.Services
{
    public class CategoryServices:ICategoryService
    {
        private UserConext _context;
        public CategoryServices(UserConext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategory()
        {
            return await _context.Categories.Where(x => !x.DeletedAt.HasValue).ToListAsync();
        }
        public async Task<Category> GetCategoryById(int id)
        {
            Category category;
            category = await _context.Categories.FirstOrDefaultAsync(u => u.Id == id && !u.DeletedAt.HasValue);
            return category;
        }

        public async Task<ResponseModel> SaveCategory(Category CategoryModel)
        {

            ResponseModel model = new ResponseModel();
            if (CategoryModel.Id==0 || CategoryModel.Id==null)
            {
                
                _context.Add<Category>(CategoryModel);
                model.IsSuccess = true;
                model.Message = "Category Inserted Successfully";
            }
            else
            {
                var category=await _context.Categories.FirstOrDefaultAsync(c=>c.Id==CategoryModel.Id);
                category.Name = CategoryModel.Name;
                category.Description = CategoryModel.Description;
                _context.Update(category);
                model.IsSuccess = true;
                model.Message = "Category Updated Successfully";

            }
            _context.SaveChanges();

            return model;
        }
        public async Task<ResponseModel> DeleteCategory(int id)
        {
            ResponseModel model = new ResponseModel();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            category.DeletedAt = DateTime.Now;
            _context.Update(category);
            model.Message = "Category Deleted Successfully";
            model.IsSuccess = true;
            _context.SaveChanges();
            return model;
        }
    }
}
