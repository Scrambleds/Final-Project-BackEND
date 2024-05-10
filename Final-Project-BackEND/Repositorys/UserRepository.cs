using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_Project_BackEND.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly TimezoneConverter _timezoneConverter;

        public UserRepository(DataContext db, TimezoneConverter timezoneConverter)
        {
            _context = db;
            _timezoneConverter = timezoneConverter;
        }

        public User GetUserByIdResultAsync(int userId)
        {
            var data = _context.Users.FromSqlInterpolated($"EXEC GetUserById {userId}").AsEnumerable().FirstOrDefault();
            return data;
        }

        public User GetUserByUsernameResult(string username)
        {
            var user = (from a in _context.Users
                        where a.username == username
                        where a.isenable == 1
                        select a).FirstOrDefault();

            return user;
        }

        public User GetUser(int userId)
        {
            var user = (from a in _context.Users
                        where a.userId == userId
                        select a).FirstOrDefault();
            return user;
        }

        public async Task<ActionResult> DeleteUser(int userId)
        {
            if (userId != 0)
            {
                var users = (from a in _context.Users
                             where a.userId == userId
                             select a).FirstOrDefault();

                if (users == null)
                {
                    return new BadRequestObjectResult(new { error = "ไม่พบผู้ใช้งานนี้ในระบบ" });
                }

                users.isenable = 0;

                try
                {
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new { user = users });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถบันทึกลงในระบบ" });
                }
            }
            else
            {
                return new BadRequestObjectResult(new { error = "ไม่พบผู้ใช้งานนี้ในระบบ" });
            }
        }


        public async Task<List<User>> GetAllUsers(string username, string email, string page)
        {
            const int pageSize = 8;
           
            var pageNumber = int.Parse(page);


            var query = (from a in _context.Users
                        where (username == null || a.username.Contains(username))
                        where (email == null || a.email.Contains(email))
                        where a.isenable == 1
                        orderby a.dateCreated descending
                        select a);

            var totalCount = await query.CountAsync();

            List<User> allUsers = new List<User>();

            if(!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(email))
            {
                allUsers = await query.ToListAsync();
            }
            else
            {
                allUsers = await query
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();
            }
            return allUsers;
        }

        public async Task<int> CountUser(string username, string email)
        {
            var count = await (from a in _context.Users
                               where (username == null || a.username.Contains(username))
                               where (email == null || a.email.Contains(email))
                               where a.isenable == 1
                               select a).CountAsync();

            return count;
        }


        public async Task<ActionResult> AddUser(User newUser)
        {
            var checkUser = await (from a in _context.Users
                                   where (a.username == newUser.username || a.email == newUser.email)
                                   where a.isenable == 1
                                   select a).FirstOrDefaultAsync();

            if (checkUser != null)
            {
                return new BadRequestObjectResult(new { error = "ชื่อผู้ใช้งาน หรือ อีเมล นี้มีในระบบเเล้ว" });
            }

            //var userDB = await (from a in _context.Users
            //                       where a.username == newUser.username
            //                       where a.email == newUser.email
            //                       select a).FirstOrDefaultAsync();

            //if (userDB != null)
            //{
            //    userDB.username = newUser.username;
            //    userDB.password = newUser.password;
            //    userDB.firstname = newUser.firstname;
            //    userDB.lastname = newUser.lastname;
            //    userDB.email = newUser.email;
            //    userDB.isAdmin = newUser.isAdmin;
            //    userDB.dateCreated = DateTime.Now;
            //    userDB.dateUpdated = null;
            //    userDB.isenable = 1;
            //    try
            //    {
            //        await _context.SaveChangesAsync();

            //        return new OkObjectResult(new { user = newUser });
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถบันทึกลงในระบบ" });
            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        await _context.Users.AddAsync(newUser);
            //        await _context.SaveChangesAsync();

            //        return new OkObjectResult(new { user = newUser });
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถบันทึกลงในระบบ" });
            //    }
            //}

            try
            {
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { user = newUser });
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถบันทึกลงในระบบ" });
            }
        }

        public async Task<ActionResult> UpdateUser(User editUser)
        {
            var userDB = await (from a in _context.Users
                                where a.userId == editUser.userId
                                select a).FirstOrDefaultAsync();

            if (userDB != null)
            {
                userDB.username = editUser.username ?? userDB.username;
                userDB.password = (editUser.password == " " ? userDB.password : editUser.password);
                userDB.firstname = editUser.firstname ?? userDB.firstname;
                userDB.lastname = editUser.lastname ?? userDB.lastname;
                userDB.email = editUser.email ?? userDB.email;
                userDB.isAdmin = editUser.isAdmin;
                userDB.dateUpdated = _timezoneConverter.ConvertToDefaultTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc);

                try
                {
                    await _context.SaveChangesAsync();

                    return new OkObjectResult(new { user = userDB });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถบันทึกลงในระบบ" });
                }
            }
            else
            {
                return new BadRequestObjectResult(new { error = "ชื่อผู้ใช้งาน หรือ อีเมล นี้มีในระบบเเล้ว" });
            }
        }

        //public async Task<ActionResult<int>> AddUser(User user)
        //{
        //    var fullNameParam = new SqlParameter("@pFullname", SqlDbType.NVarChar) { Value = user.fullname };
        //    var emailParam = new SqlParameter("@pEmail", SqlDbType.NVarChar) { Value = user.email };
        //    var ageParam = new SqlParameter("@pAge", SqlDbType.Int) { Value = user.age };
        //    var phoneParam = new SqlParameter("@pPhone", SqlDbType.NVarChar) { Value = user.phone };
        //    var addressParam = new SqlParameter("@pAddress", SqlDbType.NVarChar) { Value = user.address };

        //    var resultParam = new SqlParameter("@result", SqlDbType.Int);
        //    resultParam.Direction = ParameterDirection.Output;

        //    var result = await _context.Database.ExecuteSqlRawAsync("EXEC AddUser @pFullname, @pEmail, @pAge, @pPhone, @pAddress, @result OUTPUT", fullNameParam, emailParam, ageParam, phoneParam, addressParam,resultParam);

        //    int resultValue = (int)resultParam.Value;
        //    return resultValue;
        //}
    }
}