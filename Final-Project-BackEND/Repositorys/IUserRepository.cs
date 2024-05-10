using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Repositorys
{
    public interface IUserRepository
    {
        User GetUserByIdResultAsync(int userId);

        User GetUserByUsernameResult(string username);

        User GetUser(int userId);

        Task<ActionResult> DeleteUser(int userId);

        Task<List<User>> GetAllUsers(string username, string email, string page);
        Task<int> CountUser(string username, string email);
        Task<ActionResult> AddUser(User newUser);
        Task<ActionResult> UpdateUser(User editUser);

    }
}
