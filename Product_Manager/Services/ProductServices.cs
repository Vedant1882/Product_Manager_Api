using Microsoft.EntityFrameworkCore;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.ViewModel;

namespace Product_Manager.Services
{
    public class ProductServices:IProductServices
    {

        private UserConext _context;
        public ProductServices(UserConext context)
        {
            _context = context;
        }
        public async Task<List<ProductViewModel>> GetProduct()
        {
            List<ProductViewModel> productModel = await (from product in _context.Products.Where(x => !x.DeletedAt.HasValue)
                                        from cat in _context.Categories.Where(x => x.Id == product.CategoryId && !x.DeletedAt.HasValue)
                                        select new ProductViewModel
                                        {
                                            Id= product.Id,
                                            ProductName=product.ProductName,
                                            CategoryId=product.CategoryId,
                                            CostPrice=product.CostPrice,
                                            RetailPrice=product.RetailPrice,
                                            ManufrecturerName=product.ManufrecturerName,
                                            ManufrecturingDate =product.ManufrecturingDate,
                                            ExpiryDate=product.ExpiryDate,
                                            Status=product.Status,
                                            Name=cat.Name,
                                            ProductType=product.ProductType,
                                        }).ToListAsync();

            return productModel;
          
        }
        public async Task<ProductViewModel> GetProductById(int id)
        {
            ProductViewModel? productModel = await (from product in _context.Products.Where(x => !x.DeletedAt.HasValue && x.Id==id)
                                                         from cat in _context.Categories.Where(x => x.Id == product.CategoryId && !x.DeletedAt.HasValue)
                                                         select new ProductViewModel
                                                         {
                                                             Id = product.Id,
                                                             ProductName = product.ProductName,
                                                             CategoryId = product.CategoryId,
                                                             CostPrice = product.CostPrice,
                                                             RetailPrice = product.RetailPrice,
                                                             ManufrecturerName = product.ManufrecturerName,
                                                             ManufrecturingDate = product.ManufrecturingDate,
                                                             ExpiryDate = product.ExpiryDate,
                                                             Status = product.Status,
                                                             Name = cat.Name,
                                                             ProductType=product.ProductType,
                                                         }).FirstOrDefaultAsync();

            return productModel;
        }

        public async Task<ResponseModel> SaveProduct(ProductViewModel productModel)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(u => u.Id == productModel.CategoryId);
            ResponseModel model = new ResponseModel();
            if (productModel.Id == 0 || productModel.Id == null)
            {
                var product = new Product();
                product.CategoryId = category.Id;
                product.CostPrice = productModel.CostPrice;
                product.RetailPrice = productModel.RetailPrice;
                product.ProductName = productModel.ProductName;
                product.Status = productModel.Status;
                product.ManufrecturerName = productModel.ManufrecturerName;
                product.ExpiryDate = productModel.ExpiryDate;
                product.ManufrecturingDate = productModel.ManufrecturingDate;
                product.ImageUrl=productModel.ImageUrl;
                product.Category = category;
                product.ProductType = productModel.ProductType; 
                _context.Add<Product>(product);
                model.IsSuccess = true;
                model.Message = "Category Inserted Successfully";
            }
            else
            {
                var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == productModel.Id); ;
                product.CategoryId = category.Id;
                product.CostPrice = productModel.CostPrice;
                product.RetailPrice = productModel.RetailPrice;
                product.ProductName = productModel.ProductName;
                product.Status = productModel.Status;
                product.ManufrecturerName = productModel.ManufrecturerName;
                product.ExpiryDate = productModel.ExpiryDate;
                product.ManufrecturingDate = productModel.ManufrecturingDate;
                product.ImageUrl = productModel.ImageUrl;
                product.Category = category;
                product.ProductType = productModel.ProductType;
                _context.Update(product);
                model.IsSuccess = true;
                model.Message = "Category Inserted Successfully";

            }
            await _context.SaveChangesAsync();

            return model;
        }
        public async Task<ResponseModel> DeleteProduct(int id)
        {
            ResponseModel model = new ResponseModel();
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            product.DeletedAt = DateTime.Now;
            _context.Update(product);
            model.Message = "Category Deleted Successfully";
            model.IsSuccess = true;
            _context.SaveChanges();
            return model;
        }
    }
}
