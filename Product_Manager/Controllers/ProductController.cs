using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.ViewModel;

namespace Product_Manager.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;

        }
        [HttpPost]
        [Route("api/product/save")]
        public async Task<ResponseModel> saveProduct(ProductViewModel productViewModel)
        {
            return await _productServices.SaveProduct(productViewModel);
        }
        [HttpGet]
        [Route("api/product/getproduct")]
        public async Task<List<ProductViewModel>> GetCategoryList()
        {
            return await _productServices.GetProduct();
        }
        [HttpGet]
        [Route("api/product/getProductById/{id}")]
        public async Task<ProductViewModel> GetProductById(int id)
        {
            return await _productServices.GetProductById(id);
        }
        [HttpGet]
        [Route("api/product/deleteProduct/{id}")]
        public async Task<ResponseModel> DeleteProduct(int id)
        {
            return await _productServices.DeleteProduct(id);
        }
    }
}
