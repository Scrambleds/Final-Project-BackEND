using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Repositorys;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserById(int userId)
        {
            var data = _userRepository.GetUserByIdResultAsync(userId);
            return data;
        }

        public User GetUserByUsername(string userName)
        {
            var data = _userRepository.GetUserByUsernameResult(userName);
            return data;
        }

        public User GetUser(int userId)
        {
            var data = _userRepository.GetUser(userId);
            return data;
        }

        public async Task<ActionResult> DeleteUser(int userId)
        {
            return await _userRepository.DeleteUser(userId);
        }

        public async Task<List<User>> GetAllUsers(string username, string email, string page) 
        {
            return await _userRepository.GetAllUsers(username, email, page);
        }

        public async Task<int> CountUser(string username, string email)
        {
            return await _userRepository.CountUser(username, email);
        }

        public async Task<ActionResult> AddUser(User newUser)
        {
            return await _userRepository.AddUser(newUser);
        }

        public async Task<ActionResult> UpdateUser(User editUser)
        {
            return await _userRepository.UpdateUser(editUser);
        }

    }   
}
