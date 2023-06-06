using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.ViewModel;
using System.Net;

namespace Product_Manager.Controllers
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryServices;

        public CategoryController(ICategoryService categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpPost]
        [Route("api/category/save")]
        public async Task<ResponseModel> Save(Category categoryModel)
        {

            return await _categoryServices.SaveCategory(categoryModel);
        }
        [HttpGet]
        [Route("api/category/getCategory")]
        public async Task<List<Category>> GetCategoryList()
        {
            return await _categoryServices.GetCategory();
        }
        [HttpGet]
        [Route("api/category/getCategoryById/{id}")]
        public async Task<Category> GetCategoryById(int id)
        {
            return await _categoryServices.GetCategoryById(id);
        }
        [HttpGet]
        [Route("api/category/deleteCategory/{id}")]
        public async Task<ResponseModel> DeleteCategory(int id)
        {
            return await _categoryServices.DeleteCategory(id);
        }
    }
}
