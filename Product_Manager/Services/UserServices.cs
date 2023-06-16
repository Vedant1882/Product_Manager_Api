using Microsoft.EntityFrameworkCore;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.Providers;
using Product_Manager.ViewModel;
using System.Security.Cryptography;
using System.Text;

namespace Product_Manager.Services
{
    public class UserServices:IUserServices
    {
        private UserConext _context;
        public UserServices(UserConext context)
        {
            _context = context;
        }
        public async Task<UsersWithPage> GetUsers(tableFilter tableFilter)
        {
            List<AppUsers> users = new List<AppUsers>();
            var userLists=await _context.AppUsers.Where(x=>!x.DeletedAt.HasValue).ToListAsync();
            int pageSize = tableFilter.PageSize;
            int skip = (tableFilter.PageIndex) * pageSize;
            users = userLists.Skip(skip).Take(pageSize).ToList();
            UsersWithPage finalUsers = new UsersWithPage();
            finalUsers.data = users;
            finalUsers.totalPages = userLists.Count();
            return finalUsers;
        }
        public async Task<AppUsers> GetUsersById(int id)
        {
            AppUsers user;
            user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id && !u.DeletedAt.HasValue);
            return user;
        }
        public async Task<ResponseModel> SaveUser(UserViewModel UsersViewModel)
        {
            ResponseModel model = new ResponseModel();
            AppUsers _temp = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == UsersViewModel.Email && !u.DeletedAt.HasValue);
            if (_temp == null)
            {
                AppUsers userModel = new AppUsers();
                using var encryption = new HMACSHA512();
                userModel.Address = UsersViewModel.Address;
                userModel.Email = UsersViewModel.Email;
                userModel.FirstName = UsersViewModel.FirstName;
                userModel.LastName = UsersViewModel.LastName;
                userModel.PhoneNumber = UsersViewModel.PhoneNumber;
                userModel.Password = encryption.ComputeHash(Encoding.UTF8.GetBytes(UsersViewModel.Password));
                userModel.Key = encryption.Key;
                _context.Add<AppUsers>(userModel);
                model.IsSuccess = true;
                model.Message = "Employee Inserted Successfully";
            }
            else
            {
                //model.Message = message.emailErorMessage;
                throw new Exception(message.emailErorMessage);
            }
            _context.SaveChanges();

            return  model;
        }
    }
}
