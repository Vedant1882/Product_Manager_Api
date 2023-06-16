using Product_Manager.Models;
using Product_Manager.ViewModel;

namespace Product_Manager.Interfaces
{
    public interface IUserServices
    {
        public  Task<UsersWithPage> GetUsers(tableFilter tableFilter);
        public  Task<AppUsers> GetUsersById(int id);

        public Task<ResponseModel> SaveUser(UserViewModel UsersViewModel);


    }
}
