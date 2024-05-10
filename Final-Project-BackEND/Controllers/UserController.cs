using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Repositorys;
using Final_Project_BackEND.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Final_Project_BackEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUser")]
        public ActionResult<List<User>> GetAllUser(string? username,string? email,string? page)
        {
            var data = _userService.GetAllUsers(username, email, page).Result;
            return Ok(data);
        }

        [HttpGet("CountUser")]
        public async Task<ActionResult> CountUser(string? username, string? email)
        {
            var data = await _userService.CountUser(username, email);
            return Ok(data);
        }

        [HttpGet("GetUserById/{userId}")]
        public User GetUserById(int userId)
        {
            var user = _userService.GetUserById(userId);
            return user;
        }

        [HttpGet("GetUser/{userId}")]
        public User GetUser(int userId)
        {
            var user = _userService.GetUser(userId);
            return user;
        }


        [HttpGet("GetUserByUsername/{username}")]
        public User GetUserByUsername(string username)
        {
            var user = _userService.GetUserByUsername(username);
            return user;
        }

        [HttpPost("AddUser")]
        public ActionResult AddUser(User newUser) 
        {
            if(newUser == null) 
            {
                return BadRequest("Invalid Input");
            }

            var user = _userService.AddUser(newUser).Result;
            
            return user;
        }

        [HttpPost("UpdateUser")]
        public ActionResult UpdateUser(User editUser)
        {
            if (editUser == null)
            {
                return NotFound();
            }

            var user = _userService.UpdateUser(editUser).Result;
            return user;
        }

        [HttpPost("DeleteUser/{userId}")]
        public ActionResult DeleteUser(int userId)
        {
            if(userId == 0) {
                return NotFound();
            }

            var user = _userService.DeleteUser(userId).Result;
            return user;
        }

    }
}
