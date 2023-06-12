using Product_Manager.Models;
using Product_Manager.ViewModel;

namespace Product_Manager.Interfaces
{
    public interface IProductServices
    {
        public Task<List<ProductViewModel>> GetProduct(tableFilter tableFilterVm);
        public Task<ProductViewModel> GetProductById(int id);

        public Task<ResponseModel> SaveProduct(ProductViewModel productModel);

        public Task<ResponseModel> DeleteProduct(int id);
    }
}
