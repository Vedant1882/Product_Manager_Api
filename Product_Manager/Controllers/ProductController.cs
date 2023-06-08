using Microsoft.AspNetCore.Mvc;
using Product_Manager.Interfaces;
using Product_Manager.ViewModel;
using System.Net.Http.Headers;
using System.Web;


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
        [HttpPost]
        [Route("api/product/saveImages")]
        public async Task<ResponseModel> saveImage()
        {
            ResponseModel model=new ResponseModel();
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    model.IsSuccess = true;
                    model.Message = "file saved";
                }
                return model;
            }
            else
            {
                model.IsSuccess = false;
                model.Message = "file can't saved";
                return model;
            }
            return null;
        }
        [HttpGet]
        [Route("api/product/getproduct")]
        public async Task<List<ProductViewModel>> GetProductList()
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
