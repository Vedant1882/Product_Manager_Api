using Microsoft.EntityFrameworkCore;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.ViewModel;

namespace Product_Manager.Services
{
    public class CategoryServices : ICategoryService
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
        public async Task<CategoryWithPage> GetCategoryTable(tableFilter tableFilterVm)
        {
            List<Category> categories = new List<Category>();
            List<Category> categoryModel = new List<Category>();
            var categoryList = await _context.Categories.Where(x => !x.DeletedAt.HasValue).ToListAsync();
            if (tableFilterVm.searchValue == null || tableFilterVm.searchValue == "")
            {
                categoryModel = categoryList;
            }
            else
            {
                bool hasId = tableFilterVm.displayedHeaders.Contains("Id");
                bool hasCN = tableFilterVm.displayedHeaders.Contains("Category Name");
                bool hasDIS = tableFilterVm.displayedHeaders.Contains("Discription");
                string searchValue = tableFilterVm.searchValue.ToString().ToLower();
                categoryModel = categoryList.Where(x =>
                                                    (hasId && x.Id.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasCN && x.Name.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasDIS && x.Description.ToLower().Contains(searchValue))).ToList();
            }
            if (tableFilterVm.sortingColumnName == null || tableFilterVm.sortingDirection == "" || tableFilterVm.sortingColumnName == "" || tableFilterVm.sortingDirection == null)
            {
                categoryModel = categoryModel;
            }
            else
            {
                switch (tableFilterVm.sortingColumnName)
                {
                    case "Id":
                        categoryModel = (tableFilterVm.sortingDirection == "asc") ? categoryModel.OrderBy(x => x.Id).ToList() : categoryModel.OrderByDescending(x => x.Id).ToList();
                        break;
                    case "Category Name":
                        categoryModel = (tableFilterVm.sortingDirection == "asc") ? categoryModel.OrderBy(x => x.Name).ToList() : categoryModel.OrderByDescending(x => x.Name).ToList();
                        break;
                    case "Discription":
                        categoryModel = (tableFilterVm.sortingDirection == "asc") ? categoryModel.OrderBy(x => x.Description).ToList() : categoryModel.OrderByDescending(x => x.Description).ToList();
                        break;
                }
            }
            int pageSize = tableFilterVm.PageSize;
            int skip = (tableFilterVm.PageIndex) * pageSize;
            categories = categoryModel.Skip(skip).Take(pageSize).ToList();
            CategoryWithPage finalCategory = new CategoryWithPage();
            finalCategory.data = categories;
            finalCategory.totalPages = categoryModel.Count();
            return finalCategory;
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
            if (CategoryModel.Id == 0 || CategoryModel.Id == null)
            {

                _context.Add<Category>(CategoryModel);
                model.IsSuccess = true;
                model.Message = "Category Inserted Successfully";
            }
            else
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == CategoryModel.Id);
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
