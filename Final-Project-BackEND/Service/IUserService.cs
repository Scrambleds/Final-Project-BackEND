using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Service
{
    public interface IUserService
    {
        User GetUserById(int userId);
        User GetUserByUsername(string username);
        User GetUser(int userId);

        Task<ActionResult> DeleteUser(int userId);

        Task<List<User>> GetAllUsers(string username, string email,string page);

        Task<int> CountUser(string username, string email);

        Task<ActionResult> AddUser(User newUser);

        Task<ActionResult> UpdateUser(User newUser);

    }
}
