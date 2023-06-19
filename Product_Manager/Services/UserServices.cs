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
            List<AppUsers> usersModel = new List<AppUsers>();
            var userLists=await _context.AppUsers.Where(x=>!x.DeletedAt.HasValue).ToListAsync();
            if (tableFilter.searchValue == null || tableFilter.searchValue == "")
            {
                usersModel = userLists;
            }
            else
            {
                bool hasId = tableFilter.displayedHeaders.Contains("Id");
                bool hasFN = tableFilter.displayedHeaders.Contains("First Name");
                bool hasLN = tableFilter.displayedHeaders.Contains("Last Name");
                bool hasEMA = tableFilter.displayedHeaders.Contains("Email");
                bool hasADD = tableFilter.displayedHeaders.Contains("Address");
                bool hasPN = tableFilter.displayedHeaders.Contains("Phone Number");
                string searchValue = tableFilter.searchValue.ToString().ToLower();
                usersModel = userLists.Where(x =>
                                                    (hasId && x.Id.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasFN && x.FirstName.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasLN && x.LastName.ToLower().Contains(searchValue)) ||
                                                    (hasPN && x.PhoneNumber.ToString().ToLower().Contains(searchValue)) ||
                                                    (hasADD && x.Address.ToLower().Contains(searchValue)) ||
                                                    (hasEMA && x.Email.ToLower().Contains(searchValue))).ToList();
            }
            if (tableFilter.sortingColumnName == null || tableFilter.sortingDirection == "" || tableFilter.sortingColumnName == "" || tableFilter.sortingDirection == null)
            {
                usersModel = usersModel;
            }
            else
            {
                switch (tableFilter.sortingColumnName)
                {
                    case "Id":
                        usersModel = (tableFilter.sortingDirection == "asc") ? usersModel.OrderBy(x => x.Id).ToList() : usersModel.OrderByDescending(x => x.Id).ToList();
                        break;
                    case "First Name":
                        usersModel = (tableFilter.sortingDirection == "asc") ? usersModel.OrderBy(x => x.FirstName).ToList() : usersModel.OrderByDescending(x => x.FirstName).ToList();
                        break;
                    case "Last Name":
                        usersModel = (tableFilter.sortingDirection == "asc") ? usersModel.OrderBy(x => x.LastName).ToList() : usersModel.OrderByDescending(x => x.LastName).ToList();
                        break;
                    case "Email":
                        usersModel = (tableFilter.sortingDirection == "asc") ? usersModel.OrderBy(x => x.Email).ToList() : usersModel.OrderByDescending(x => x.Email).ToList();
                        break;
                    case "Address":
                        usersModel = (tableFilter.sortingDirection == "asc") ? usersModel.OrderBy(x => x.Address).ToList() : usersModel.OrderByDescending(x => x.Address).ToList();
                        break;
                    case "Phone Number":
                        usersModel = (tableFilter.sortingDirection == "asc") ? usersModel.OrderBy(x => x.PhoneNumber).ToList() : usersModel.OrderByDescending(x => x.PhoneNumber).ToList();
                        break;
                }
            }
            int pageSize = tableFilter.PageSize;
            int skip = (tableFilter.PageIndex) * pageSize;
            users = usersModel.Skip(skip).Take(pageSize).ToList();
            UsersWithPage finalUsers = new UsersWithPage();
            finalUsers.data = users;
            finalUsers.totalPages = usersModel.Count();
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
            if(UsersViewModel.Id==0 || UsersViewModel.Id == null)
            {
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
            }
            else
            {
                    AppUsers userModel =await _context.AppUsers.FirstOrDefaultAsync(u=>u.Id==UsersViewModel.Id);
                    userModel.Address = UsersViewModel.Address;
                    userModel.Email = UsersViewModel.Email;
                    userModel.FirstName = UsersViewModel.FirstName;
                    userModel.LastName = UsersViewModel.LastName;
                    userModel.PhoneNumber = UsersViewModel.PhoneNumber;
                    _context.Update<AppUsers>(userModel);
                    model.IsSuccess = true;
                    model.Message = "Employee Inserted Successfully";
               
            }
            _context.SaveChanges();

            return  model;
        }
        public async Task<ResponseModel> DeleteUser(int id)
        {
            ResponseModel model = new ResponseModel();
            var user = await _context.AppUsers.FirstOrDefaultAsync(c => c.Id == id);
            user.DeletedAt = DateTime.Now;
            _context.Update(user);
            model.Message = "Category Deleted Successfully";
            model.IsSuccess = true;
            _context.SaveChanges();
            return model;
        }
    }
}
