using Microsoft.EntityFrameworkCore;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.ViewModel;

namespace Product_Manager.Services
{
    public class ProductServices : IProductServices
    {

        private UserConext _context;
        public ProductServices(UserConext context)
        {
            _context = context;
        }
        public async Task<List<ProductViewModel>> GetProduct(tableFilter tableFilterVm)
       {
            List<ProductViewModel> products = new List<ProductViewModel>();
            List<ProductViewModel> productModel = new List<ProductViewModel>();
            
            var productsList =await (from product in _context.Products.Where(x => !x.DeletedAt.HasValue)
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
                                ProductType = product.ProductType,
                            }).ToListAsync();

            if (tableFilterVm.searchValue == null || tableFilterVm.searchValue == "")
            {
                productModel = productsList;
            }
            else {
                bool hasId = tableFilterVm.displayedHeaders.Contains("Id");
                bool hasPN = tableFilterVm.displayedHeaders.Contains("Product Name");
                bool hasCP = tableFilterVm.displayedHeaders.Contains("Cost Price");
                bool hasRP = tableFilterVm.displayedHeaders.Contains("Retail Price");
                bool hasMN = tableFilterVm.displayedHeaders.Contains("Manufracturer Name");
                bool hasMD = tableFilterVm.displayedHeaders.Contains("Manufrecturing Date");
                bool hasED = tableFilterVm.displayedHeaders.Contains("Expiry Date");
                bool hasStatus = tableFilterVm.displayedHeaders.Contains("Satus");
                bool hasPT = tableFilterVm.displayedHeaders.Contains("Product Type");
                string searchValue = tableFilterVm.searchValue.ToString().ToLower();
                productModel = productsList.Where(x =>
                                                    (hasId && x.Id.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasPN && x.ProductName.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasCP && x.CostPrice.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasRP && x.RetailPrice.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasMN && x.ManufrecturerName.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasMD && x.ManufrecturingDate.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasED && x.ExpiryDate.HasValue && x.ExpiryDate.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasStatus && x.Status.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasPT && x.ProductType.ToLower().Contains(searchValue))).ToList();
                    
            }
            

            
            int pageSize = tableFilterVm.PageSize;
            int skip = (tableFilterVm.PageIndex) * pageSize;
            products = productModel.Skip(skip).Take(pageSize).ToList();
            return products;

        }
        public async Task<ProductViewModel> GetProductById(int id)
        {
            ProductViewModel? productModel = await (from product in _context.Products.Where(x => !x.DeletedAt.HasValue && x.Id == id)
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
                                                        ProductType = product.ProductType,
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
                product.ImageUrl = productModel.ImageUrl;
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
