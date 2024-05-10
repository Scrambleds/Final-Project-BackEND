using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_Project_BackEND.Repositorys
{
    public class ListRepository : IListRepository
    {
        private readonly DataContext _context;
        private readonly TimezoneConverter _timezoneConverter;

        public ListRepository(DataContext db, TimezoneConverter timezoneConverter)
        {
            _context = db;
            _timezoneConverter = timezoneConverter;
        }

        public async Task<List<ImportHeader>> GetAlllistimportheader()
        {
            var data = await _context.importHeaders.ToListAsync();
            return data;
        }

        public async Task<List<ImportHeader>> Getlistimportheader(string? ImportHeaderNo, string? CourseID, string? YearEducation, string? page, int userId)
        {
            IQueryable<ImportHeader> query;
            const int pageSize = 8;

            var pageNumber = int.Parse(page);
            var checkAdmin = (from a in _context.Users
                              where a.userId == userId
                              select new
                              {
                                  a.isAdmin
                              }).FirstOrDefault();

            if (checkAdmin.isAdmin == 1)
            {
                query = (from a in _context.importHeaders
                         where (string.IsNullOrEmpty(ImportHeaderNo) || a.importHeaderNumber.Contains(ImportHeaderNo))
                         where (string.IsNullOrEmpty(CourseID) || a.courseID.Contains(CourseID))
                         where (string.IsNullOrEmpty(YearEducation) || a.yearEducation.Contains(YearEducation))
                         orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                         select a);
            }
            else
            {
                query = (from a in _context.importHeaders
                         where (string.IsNullOrEmpty(ImportHeaderNo) || a.importHeaderNumber.Contains(ImportHeaderNo))
                         where (string.IsNullOrEmpty(CourseID) || a.courseID.Contains(CourseID))
                         where (string.IsNullOrEmpty(YearEducation) || a.yearEducation.Contains(YearEducation))
                         where a.createByUserId == userId
                         orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                         select a);
            }

            var totalCount = await query.CountAsync();

            List<ImportHeader> allList = new List<ImportHeader>();

            if (!string.IsNullOrEmpty(ImportHeaderNo) || !string.IsNullOrEmpty(CourseID) || !string.IsNullOrEmpty(YearEducation))
            {
                allList = await query.ToListAsync();

            }
            else
            {
                allList = await query
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();

            }

            foreach (var item in allList)
            {
                if (item != null)
                {
                    item.dateCreated = _timezoneConverter.ConvertToDefaultTimeZone((DateTime)(item.dateUpdated.HasValue ? item.dateUpdated : item.dateCreated), TimeZoneInfo.Utc);
                }
            }

            return allList;
        }

        public async Task<List<ImportHeader>> GetlistimportheaderForPage(string page, int userId)
        {
            const int pageSize = 5;
            IQueryable<ImportHeader> query;

            var pageNumber = int.Parse(page);

            var checkAdmin = (from a in _context.Users
                              where a.userId == userId
                              select new
                              {
                                  a.isAdmin
                              }).FirstOrDefault();

            if (checkAdmin.isAdmin == 1)
            {
                query = (from a in _context.importHeaders
                         where a.isenable == 1
                         orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                         select a);
            }
            else
            {
                query = (from a in _context.importHeaders
                         where a.isenable == 1
                         where a.createByUserId == userId
                         orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                         select a);
            }

            var totalCount = await query.CountAsync();

            var allList = await query
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();
            foreach (var item in allList)
            {
                if (item != null)
                {
                    item.dateCreated = _timezoneConverter.ConvertToDefaultTimeZone((DateTime)(item.dateUpdated.HasValue ? item.dateUpdated : item.dateCreated), TimeZoneInfo.Utc);
                }
            }

            return allList;
        }

        public async Task<int> CountListimportheader(string? importNo, string? courseID, int userId)
        {
            int count = new int();
            var checkAdmin = (from a in _context.Users
                              where a.userId == userId
                              select new
                              {
                                  a.isAdmin
                              }).FirstOrDefault();

            if (checkAdmin.isAdmin == 1)
            {
                count = await (from a in _context.importHeaders
                               where (string.IsNullOrEmpty(importNo) || a.importHeaderNumber.Contains(importNo))
                               where (string.IsNullOrEmpty(courseID) || a.courseID.Contains(courseID))
                               where a.isenable == 1
                               select a).CountAsync();
            }
            else
            {
                count = await (from a in _context.importHeaders
                               where (string.IsNullOrEmpty(importNo) || a.importHeaderNumber.Contains(importNo))
                               where (string.IsNullOrEmpty(courseID) || a.courseID.Contains(courseID))
                               where a.isenable == 1
                               where a.createByUserId == userId
                               select a).CountAsync();
            }

            return count;
        }

        public async Task<List<ImportHeader>> GetlistimportheaderByFilter(importHSearch filter)
        {
            var query = _context.importHeaders.AsQueryable();

            if (filter.importHeaderID.HasValue)
                query = query.Where(importHeader => importHeader.importHeaderID == filter.importHeaderID);

            if (!string.IsNullOrEmpty(filter.importHeaderNumber))
                query = query.Where(importHeader => importHeader.importHeaderNumber.Contains(filter.importHeaderNumber));

            if (!string.IsNullOrEmpty(filter.courseID))
                query = query.Where(importHeader => importHeader.courseID == filter.courseID);

            if (!string.IsNullOrEmpty(filter.courseName))
                query = query.Where(importHeader => importHeader.courseName.Contains(filter.courseName));

            if (!string.IsNullOrEmpty(filter.semester))
                query = query.Where(importHeader => importHeader.semester == filter.semester);

            if (!string.IsNullOrEmpty(filter.yearEducation))
                query = query.Where(importHeader => importHeader.yearEducation == filter.yearEducation);

            if (filter.dateCreated.HasValue)
            {
                var dateCreatedWithoutTime = filter.dateCreated.Value.Date;
                query = query.Where(importHeader => importHeader.dateCreated.Date == dateCreatedWithoutTime);
            }

            if (filter.dateUpdated.HasValue)
            {
                var dateUpdatedWithoutTime = filter.dateUpdated.Value.Date;
                query = query.Where(importHeader => importHeader.dateUpdated != null && importHeader.dateUpdated.Value.Date == dateUpdatedWithoutTime);
            }

            var data = await query.ToListAsync();
            return data;
        }

        public async Task<int> DeleteImportList(string importHeaderNumber)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var sqlCommand = $"DELETE FROM importHeaders WHERE importHeaderNumber = @ImportHeaderNumber";
                    var result = await _context.Database.ExecuteSqlRawAsync(sqlCommand, new SqlParameter("@ImportHeaderNumber", importHeaderNumber));

                    if (result == 1)
                    {
                        var listOfStudent = await (from a in _context.Excels
                                                   where a.importHeaderNumber == importHeaderNumber
                                                   select a).ToListAsync();

                        var gradeOfImportHeader = await (from a in _context.GradeImportHeaders
                                                         where a.importHeaderNumber == importHeaderNumber
                                                         select a).FirstOrDefaultAsync();

                        // Step 2: Delete related record from Excels
                        if (listOfStudent != null)
                        {
                            _context.Excels.RemoveRange(listOfStudent);
                        }

                        if (gradeOfImportHeader != null)
                        {
                            _context.GradeImportHeaders.Remove(gradeOfImportHeader);
                        }
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<ImportHeader> GetDetailByHeaderNo(string importHeaderNumber)
        {
            var result = await (from a in _context.importHeaders
                                where a.importHeaderNumber == importHeaderNumber
                                select a).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ImportHeader>> GetDetailByHeaderNoByUserId(string ImportHeaderNo, int userId)
        {
            

            var checkAdmin = await (from a in _context.Users
                                    where a.userId == userId
                                    select new
                                    {
                                        a.isAdmin
                                    }).FirstOrDefaultAsync();

            if(checkAdmin.isAdmin ==1)
            {
                var result = await (from a in _context.importHeaders
                                    where a.importHeaderNumber == ImportHeaderNo
                                    where a.isenable == 1
                                    select a).ToListAsync();
                foreach (var item in result)
                {
                    item.dateCreated = _timezoneConverter.ConvertToDefaultTimeZone((DateTime)(item.dateUpdated.HasValue ? item.dateUpdated : item.dateCreated), TimeZoneInfo.Utc);
                }

                return result;
            }
            else
            {
                var result = await (from a in _context.importHeaders
                                    where a.importHeaderNumber == ImportHeaderNo
                                    where a.createByUserId == userId
                                    where a.isenable == 1
                                    select a).ToListAsync();
                foreach (var item in result)
                {
                    item.dateCreated = _timezoneConverter.ConvertToDefaultTimeZone((DateTime)(item.dateUpdated.HasValue ? item.dateUpdated : item.dateCreated), TimeZoneInfo.Utc);
                }

                return result;
            }
        }

        public async Task<JsonResult> GetExcelDetail(string importHeaderNumber)
        {
            try
            {
                var data = await (from a in _context.Excels
                                  where a.importHeaderNumber == importHeaderNumber
                                  select new
                                  {
                                      a.no,
                                      a.id,
                                      a.name,
                                      a.grade
                                  }).ToListAsync();

                if (data == null || !data.Any())
                {
                    return new JsonResult(new { error = "ไม่มีข้อมูลในระบบ" });
                }
                else
                {
                    return new JsonResult(data);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return new JsonResult(new { error = "เกิดข้อผิดพลาดบางอย่าง" });
            }
        }

        public async Task<JsonResult> GetExcelDetailByHeaderNoByUserId(string ImportHeaderNo, int userId)
        {
            IQueryable<object> query;
            try
            {
                var checkAdmin = (from a in _context.Users
                                  where a.userId == userId
                                  select new
                                  {
                                      a.isAdmin
                                  }).FirstOrDefault();

                if (checkAdmin.isAdmin == 1)
                {
                    query = (from a in _context.Excels
                             where a.importHeaderNumber == ImportHeaderNo
                             select new
                             {
                                 a.no,
                                 a.id,
                                 a.name,
                                 a.grade
                             });
                }
                else
                {
                    query = (from a in _context.Excels
                             where a.importHeaderNumber == ImportHeaderNo
                             where a.createByUserId == userId
                             select new
                             {
                                 a.no,
                                 a.id,
                                 a.name,
                                 a.grade
                             });
                }

                var result = await query.ToListAsync();

                if (result == null || !result.Any())
                {
                    return new JsonResult(new { error = "ไม่มีข้อมูลในระบบ" });
                }
                else
                {
                    return new JsonResult(result);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return new JsonResult(new { error = "เกิดข้อผิดพลาดบางอย่าง" });
            }
        }

        public async Task<List<GroupStudentInExcel>> GetAllStudent()
        {
            var groupStudentData = await (from a in _context.Excels
                                          group a by new
                                          {
                                              a.id,
                                              a.name
                                          } into group1
                                          where group1.Count() >= 1
                                          select new GroupStudentInExcel
                                          {
                                              id = group1.Key.id,
                                              name = group1.Key.name,
                                              count = group1.Count()
                                          }).ToListAsync();

            if (groupStudentData.Count < 1)
            {
                return null;
            }

            return groupStudentData;
        }

        public async Task<List<GroupStudentInExcel>> GetAllStudentForPage(string? id, string? name, string? page, int userId)
        {
            const int pageSize = 8;
            IQueryable<GroupStudentInExcel> query;

            var pageNumber = int.Parse(page);

            var checkAdmin = await (from a in _context.Users
                                    where a.userId == userId
                                    select new
                                    {
                                        a.isAdmin
                                    }).FirstOrDefaultAsync();



            if (checkAdmin.isAdmin == 1)
            {
                query = (from a in _context.Excels
                         where (string.IsNullOrEmpty(id) || a.id.Contains(id))
                         where (string.IsNullOrEmpty(name) || a.name.Contains(name))
                         group a by new
                         {
                             a.id,
                             a.name
                         } into group1
                         where group1.Count() >= 1
                         orderby (string.IsNullOrEmpty(group1.Key.id) ? "" : group1.Key.id) ascending
                         select new GroupStudentInExcel
                         {
                             id = group1.Key.id,
                             name = group1.Key.name,
                             count = group1.Count()
                         });


            }
            else
            {
                query = (from a in _context.Excels
                         where (string.IsNullOrEmpty(id) || a.id.Contains(id))
                         where (string.IsNullOrEmpty(name) || a.name.Contains(name))
                         where a.createByUserId == userId
                         group a by new
                         {
                             a.id,
                             a.name
                         } into group1
                         where group1.Count() >= 1
                         orderby (string.IsNullOrEmpty(group1.Key.id) ? "" : group1.Key.id) ascending
                         select new GroupStudentInExcel
                         {
                             id = group1.Key.id,
                             name = group1.Key.name,
                             count = group1.Count()
                         });
            }

            var totalCount = await query.CountAsync();

            List<GroupStudentInExcel> allList = new List<GroupStudentInExcel>();

            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(name))
            {
                 allList = await query.ToListAsync();
              
            }
            else
            {
                 allList = await query
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();
               
            }
            return allList;
        }

        public async Task<int> CountAllStudent(string? id, string? name, int userId)
        {
            int count = new int();
            var checkAdmin = (from a in _context.Users
                              where a.userId == userId
                              select new
                              {
                                  a.isAdmin
                              }).FirstOrDefault();

            if (checkAdmin.isAdmin == 1)
            {
                count = await (from a in _context.Excels
                               where (string.IsNullOrEmpty(id) || a.id.Contains(id))
                               where (string.IsNullOrEmpty(name) || a.name.Contains(name))
                               group a by new
                               {
                                   a.id,
                                   a.name
                               } into group1
                               where group1.Count() >= 1
                               select new
                               {
                                   id = group1.Key.id,
                                   name = group1.Key.name,
                                   count = group1.Count()
                               }).Distinct().CountAsync();
            }
            else
            {
                count = await (from a in _context.Excels
                               where (string.IsNullOrEmpty(id) || a.id.Contains(id))
                               where (string.IsNullOrEmpty(name) || a.name.Contains(name))
                               where a.createByUserId == userId
                               group a by new
                               {
                                   a.id,
                                   a.name
                               } into group1
                               where group1.Count() >= 1
                               select new
                               {
                                   id = group1.Key.id,
                                   name = group1.Key.name,
                                   count = group1.Count()
                               }).Distinct().CountAsync();
            }

            return count;
        }

        public async Task<List<GradeFromId>> GetGradeStudent(string? id)
        {

            var ListStudent = await (from a in _context.importHeaders
                                     join b in _context.Excels on a.importHeaderNumber equals b.importHeaderNumber
                                     where b.id == id
                                     orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                                     select new GradeFromId
                                     {
                                         courseID = a.courseID,
                                         courseName = a.courseName,
                                         semester = a.semester,
                                         yearEducation = a.yearEducation,
                                         grade = b.grade,
                                         name = b.name,
                                         id = b.id
                                     }).ToListAsync();

            if (ListStudent.Count < 1)
            {
                return null;
            }

            return ListStudent;
        }

        public async Task<List<GradeFromId>> GetGradeStudentByUserId(string? id, int userId)
        {
            IQueryable<GradeFromId> query;
            var checkAdmin = await (from a in _context.Users
                                    where a.userId == userId
                                    select new
                                    {
                                        a.isAdmin
                                    }).FirstOrDefaultAsync();



            if (checkAdmin.isAdmin == 1)
            {
                query = (from a in _context.importHeaders
                         join b in _context.Excels on a.importHeaderNumber equals b.importHeaderNumber
                         where b.id == id
                         orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                         select new GradeFromId
                         {
                             courseID = a.courseID,
                             courseName = a.courseName,
                             semester = a.semester,
                             yearEducation = a.yearEducation,
                             grade = b.grade,
                             name = b.name,
                             id = b.id
                         });
            }
            else
            {
                query = (from a in _context.importHeaders
                         join b in _context.Excels on a.importHeaderNumber equals b.importHeaderNumber
                         where a.createByUserId == userId
                         where b.id == id
                         orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                         select new GradeFromId
                         {
                             courseID = a.courseID,
                             courseName = a.courseName,
                             semester = a.semester,
                             yearEducation = a.yearEducation,
                             grade = b.grade,
                             name = b.name,
                             id = b.id
                         });
            }


            var allList = await query.ToListAsync();
            if (allList.Count < 1)
            {
                return null;
            }

            return allList;
        }

        public async Task<JsonResult> SumGradeStudent(string? id)
        {
            try
            {
                var data = await (from a in _context.Excels
                                  where a.id == id
                                  orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                                  select new
                                  {
                                      a.id,
                                      a.name,
                                      a.grade
                                  }).ToListAsync();

                // สร้าง Dictionary เพื่อเก็บจำนวนของแต่ละเกรด
                Dictionary<string, int> gradeCount = new Dictionary<string, int>();

                // นับจำนวนของแต่ละเกรด
                foreach (var item in data)
                {
                    // ถ้าเกรดนั้นๆยังไม่มีใน Dictionary ให้เพิ่มเข้าไปและกำหนดค่าเริ่มต้นเป็น 1
                    if (!gradeCount.ContainsKey(item.grade))
                    {
                        gradeCount[item.grade] = 1;
                    }
                    // ถ้าเกรดนั้นๆมีใน Dictionary แล้ว ให้เพิ่มค่าจำนวนขึ้นไป
                    else
                    {
                        gradeCount[item.grade]++;
                    }
                }

                // สร้าง Object เพื่อเก็บผลลัพธ์f
                var result = new
                {
                    id = id,
                    name = data.FirstOrDefault()?.name,
                    a = gradeCount.GetValueOrDefault("A", 0).ToString(),
                    bPlus = gradeCount.GetValueOrDefault("B+", 0).ToString(),
                    b = gradeCount.GetValueOrDefault("B", 0).ToString(),
                    cPlus = gradeCount.GetValueOrDefault("C+", 0).ToString(),
                    c = gradeCount.GetValueOrDefault("C", 0).ToString(),
                    dPlus = gradeCount.GetValueOrDefault("D+", 0).ToString(),
                    d = gradeCount.GetValueOrDefault("D", 0).ToString(),
                    i = gradeCount.GetValueOrDefault("I", 0).ToString(),
                    f = gradeCount.GetValueOrDefault("F", 0).ToString(),
                    w = gradeCount.GetValueOrDefault("W", 0).ToString(),
                    total = gradeCount.Values.Sum().ToString()
                };

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = $"เกิดข้อผิดพลาด: {ex.Message}" });
            }
        }

        public async Task<JsonResult> GetDropdownYearEducation(int userId)
        {
            IQueryable<object> query;
            try
            {
                var checkAdmin = await (from a in _context.Users
                                        where a.userId == userId
                                        select new
                                        {
                                            a.isAdmin
                                        }).FirstOrDefaultAsync();
                if(checkAdmin.isAdmin == 1)
                {
                    query = (from a in _context.importHeaders
                             select a.yearEducation).Distinct();
                }
                else
                {
                    query =  (from a in _context.importHeaders
                                   where a.createByUserId == userId
                                   select a.yearEducation).Distinct();
                }
                var result = await query.ToListAsync();
                return new JsonResult(result);
            }
            catch (Exception ex)
            {

                return new JsonResult(new { error = $"เกิดข้อผิดพลาด: {ex.Message}" });
            }
        }


        public async Task<JsonResult> SumGradeStudentByUserId(string? id, int userId)
        {
            try
            {
                IQueryable<object> query;
                var checkAdmin = await (from a in _context.Users
                                        where a.userId == userId
                                        select new
                                        {
                                            a.isAdmin
                                        }).FirstOrDefaultAsync();

                object result = null; // ประกาศตัวแปร result นอกจาก if-else

                if (checkAdmin.isAdmin == 1)
                {
                    var data = await (from a in _context.Excels
                                      where a.id == id
                                      orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                                      select new
                                      {
                                          a.id,
                                          a.name,
                                          a.grade
                                      }).ToListAsync();

                    // สร้าง Dictionary เพื่อเก็บจำนวนของแต่ละเกรด
                    Dictionary<string, int> gradeCount = new Dictionary<string, int>();

                    // นับจำนวนของแต่ละเกรด
                    foreach (var item in data)
                    {
                        // ถ้าเกรดนั้นๆยังไม่มีใน Dictionary ให้เพิ่มเข้าไปและกำหนดค่าเริ่มต้นเป็น 1
                        if (!gradeCount.ContainsKey(item.grade))
                        {
                            gradeCount[item.grade] = 1;
                        }
                        // ถ้าเกรดนั้นๆมีใน Dictionary แล้ว ให้เพิ่มค่าจำนวนขึ้นไป
                        else
                        {
                            gradeCount[item.grade]++;
                        }
                    }

                    // สร้าง Object เพื่อเก็บผลลัพธ์
                    result = new
                    {
                        id = id,
                        name = data.FirstOrDefault()?.name,
                        a = gradeCount.GetValueOrDefault("A", 0).ToString(),
                        bPlus = gradeCount.GetValueOrDefault("B+", 0).ToString(),
                        b = gradeCount.GetValueOrDefault("B", 0).ToString(),
                        cPlus = gradeCount.GetValueOrDefault("C+", 0).ToString(),
                        c = gradeCount.GetValueOrDefault("C", 0).ToString(),
                        dPlus = gradeCount.GetValueOrDefault("D+", 0).ToString(),
                        d = gradeCount.GetValueOrDefault("D", 0).ToString(),
                        i = gradeCount.GetValueOrDefault("I", 0).ToString(),
                        f = gradeCount.GetValueOrDefault("F", 0).ToString(),
                        w = gradeCount.GetValueOrDefault("W", 0).ToString(),
                        total = gradeCount.Values.Sum().ToString()
                    };
                }
                else
                {
                    var data = await (from a in _context.Excels
                                      where a.id == id
                                      where a.createByUserId == userId
                                      orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                                      select new
                                      {
                                          a.id,
                                          a.name,
                                          a.grade
                                      }).ToListAsync();

                    // สร้าง Dictionary เพื่อเก็บจำนวนของแต่ละเกรด
                    Dictionary<string, int> gradeCount = new Dictionary<string, int>();

                    // นับจำนวนของแต่ละเกรด
                    foreach (var item in data)
                    {
                        // ถ้าเกรดนั้นๆยังไม่มีใน Dictionary ให้เพิ่มเข้าไปและกำหนดค่าเริ่มต้นเป็น 1
                        if (!gradeCount.ContainsKey(item.grade))
                        {
                            gradeCount[item.grade] = 1;
                        }
                        // ถ้าเกรดนั้นๆมีใน Dictionary แล้ว ให้เพิ่มค่าจำนวนขึ้นไป
                        else
                        {
                            gradeCount[item.grade]++;
                        }
                    }

                    // สร้าง Object เพื่อเก็บผลลัพธ์
                    result = new
                    {
                        id = id,
                        name = data.FirstOrDefault()?.name,
                        a = gradeCount.GetValueOrDefault("A", 0).ToString(),
                        bPlus = gradeCount.GetValueOrDefault("B+", 0).ToString(),
                        b = gradeCount.GetValueOrDefault("B", 0).ToString(),
                        cPlus = gradeCount.GetValueOrDefault("C+", 0).ToString(),
                        c = gradeCount.GetValueOrDefault("C", 0).ToString(),
                        dPlus = gradeCount.GetValueOrDefault("D+", 0).ToString(),
                        d = gradeCount.GetValueOrDefault("D", 0).ToString(),
                        i = gradeCount.GetValueOrDefault("I", 0).ToString(),
                        f = gradeCount.GetValueOrDefault("F", 0).ToString(),
                        w = gradeCount.GetValueOrDefault("W", 0).ToString(),
                        total = gradeCount.Values.Sum().ToString()
                    };
                }

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = $"เกิดข้อผิดพลาด: {ex.Message}" });
            }
        }




        public async Task<List<GradeImportHeader>> GetGradeImportHeaderNumber(string? importHeaderNumber)
        {
            IQueryable<GradeImportHeader> query;
            query = (from a in _context.GradeImportHeaders
                     where a.importHeaderNumber == importHeaderNumber
                     orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                     select a);

            var allList = await query.ToListAsync();
            if (allList.Count < 1)
            {
                return null;
            }

            return allList;
        }


        public async Task<List<GradeImportHeader>> GetGradeImportHeaderNumberByUserId(string? importHeaderNumber, int userId)
        {
            IQueryable<GradeImportHeader> query;
            var checkAdmin = await (from a in _context.Users
                                    where a.userId == userId
                                    select new
                                    {
                                        a.isAdmin
                                    }).FirstOrDefaultAsync();



            if (checkAdmin.isAdmin == 1)
            {
                query = (from a in _context.GradeImportHeaders
                         where a.importHeaderNumber == importHeaderNumber
                         orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                         select a);
            }
            else
            {
                query = (from a in _context.GradeImportHeaders
                         where a.importHeaderNumber == importHeaderNumber
                         where a.createByUserId == userId
                         orderby (a.dateUpdated != null ? a.dateUpdated : a.dateCreated) descending
                         select a);
            }

            var allList = await query.ToListAsync();
            if (allList.Count < 1)
            {
                return null;
            }

            return allList;
        }
    }
}
