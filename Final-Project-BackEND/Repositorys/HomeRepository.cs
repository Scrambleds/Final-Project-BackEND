using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Final_Project_BackEND.Repositorys
{
    public class HomeRepository : IHomeRepository
    {
        private readonly DataContext _context;
        private readonly TimezoneConverter _timezoneConverter;


        public HomeRepository(DataContext db, TimezoneConverter timezoneConverter)
        {
            _context = db;
            _timezoneConverter = timezoneConverter;
        }

        public async Task<ActionResult<List<ModelGrade>>> GetDataDashboard(string? CourseName, string? CourseID, string? YearEducation, string? Semester, int userId)
        {
            List<Excel> excelList = new List<Excel>();
            try
            {
                var checkAdmin = await (from a in _context.Users
                                        where a.userId == userId
                                        select a).FirstOrDefaultAsync();

                if(checkAdmin.isAdmin == 1)
                {
                    excelList = await (from a in _context.Excels
                                        join b in _context.importHeaders on a.importHeaderNumber equals b.importHeaderNumber
                                        where (string.IsNullOrEmpty(CourseName) ? true : b.courseName.Contains(CourseName))
                                           && (string.IsNullOrEmpty(CourseID) ? true : b.courseID.Contains(CourseID))
                                           && (string.IsNullOrEmpty(YearEducation) ? true : b.yearEducation.Contains(YearEducation))
                                           && (string.IsNullOrEmpty(Semester) ? true : b.semester.Contains(Semester))
                                        select a).ToListAsync();
                }
                else
                {
                    excelList = await (from a in _context.Excels
                                       join b in _context.importHeaders on a.importHeaderNumber equals b.importHeaderNumber
                                       where (string.IsNullOrEmpty(CourseName) ? true : b.courseName.Contains(CourseName))
                                          && (string.IsNullOrEmpty(CourseID) ? true : b.courseID.Contains(CourseID))
                                          && (string.IsNullOrEmpty(YearEducation) ? true : b.yearEducation.Contains(YearEducation))
                                          && (string.IsNullOrEmpty(Semester) ? true : b.semester.Contains(Semester))
                                       where b.createByUserId == userId
                                       select a).ToListAsync();
                }


                if (excelList == null || !excelList.Any())
                {
                    return new BadRequestObjectResult(new { error = "ไม่พบข้อมูล" });
                }
                else
                {
                    var gradeLabels = _context.GradeFilter.ToList();
                    var totalStudents = excelList.Count;
                    var grades = excelList.Select(student => student.grade).ToList();

                    var gradeCounts = grades.GroupBy(grade => grade)
                                            .ToDictionary(group => group.Key, group => group.Count());

                    var percentageData = gradeLabels.Select(grade =>
                        new ModelGrade
                        {
                            GradeString = grade.Grade,
                            Percentage = (decimal?)(gradeCounts.ContainsKey(grade.Grade ?? string.Empty) ? (gradeCounts[grade.Grade ?? string.Empty] / (double)totalStudents) * 100 : 0)
                        }).ToList();

                    return percentageData;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถดึงข้อมูลในระบบได้" });
            }
        }
        public async Task<JsonResult> GetYearEducationList(int userId)
        {
            IQueryable<object> data;
            try
            {
                var checkAdmin = await (from a in _context.Users
                                        where userId == a.userId
                                        select a).FirstOrDefaultAsync();
                if (checkAdmin.isAdmin == 1) 
                {
                    data = _context.importHeaders.Select(a => new
                                    {
                                        a.yearEducation,
                                    })
                                    .Distinct();
                }
                else
                {
                    data = _context.importHeaders.Where(a => a.createByUserId == userId)
                                    .Select(a => new
                                    {
                                       a.yearEducation,
                                    })
                                    .Distinct();

                }
                var listYearEducations = await data.ToListAsync();

                if (listYearEducations == null || !listYearEducations.Any())
                {
                    return new JsonResult(new { error = "ไม่มีข้อมูลในระบบ" });
                }
                else
                {
                    return new JsonResult(listYearEducations);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return new JsonResult(new { error = "เกิดข้อผิดพลาดบางอย่าง" });
            }
        }

        public async Task<JsonResult> FilterSearchSemester(string? yearEducation, string? semester, int userid)
        {
            IQueryable<object> data;
            try
            {
                var checkAdmin = await (from a in _context.Users
                                        where userid == a.userId
                                        select a).FirstOrDefaultAsync();
                if (checkAdmin.isAdmin == 1)
                {
                    data = (from a in _context.importHeaders
                            where a.yearEducation == yearEducation
                            where a.semester == semester
                            select new
                            {
                                CourseIdAndCourseName = (a.courseID + "-" + a.courseName)
                            }).Distinct();
                }
                else
                {
                    data = (from a in _context.importHeaders
                            where a.yearEducation == yearEducation
                            where a.semester == semester
                            where a.createByUserId == userid
                            select new
                            {
                                CourseIdAndCourseName = (a.courseID + "-" + a.courseName)
                            }).Distinct();
                }

                var result = await data.ToListAsync();
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

        public async Task<JsonResult> FilterSearchYear(string? yearEducation, int userid)
        {
            IQueryable<object> data;
            try
            {
                var checkAdmin = await (from a in _context.Users
                                        where userid == a.userId
                                        select a).FirstOrDefaultAsync();
                if (checkAdmin.isAdmin == 1)
                {
                    data = (from a in _context.importHeaders
                            where a.yearEducation == yearEducation
                            select new
                            {
                                a.semester
                            }).Distinct();
                }
                else
                {
                    data = (from a in _context.importHeaders
                            where a.yearEducation == yearEducation
                            where a.createByUserId == userid
                            select new
                            {
                                a.semester
                            }).Distinct();
                }

                var result = await data.ToListAsync();
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

        public async Task<JsonResult> GetDataTable()
        {
            try
            {
                var data = await (from a in _context.importHeaders
                                  join b in _context.GradeImportHeaders on a.importHeaderNumber equals b.importHeaderNumber
                                  select new
                                  {
                                      a.importHeaderNumber,
                                      a.courseID,
                                      a.courseName,
                                      a.semester,
                                      b.Total,
                                      b.A,
                                      b.Bplus,
                                      b.B,
                                      b.Cplus,
                                      b.C,
                                      b.Dplus,
                                      b.D,
                                      b.F,
                                      b.I,
                                      b.W,
                                      dateCreated = _timezoneConverter.ConvertToDefaultTimeZone((DateTime)(b.dateUpdated.HasValue ? b.dateUpdated : b.dateCreated), TimeZoneInfo.Utc),
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

        public async Task<JsonResult> GetDataTableByUserId(string? CourseName, string? CourseID, string? YearEducation, string? Semester, int userId)
        {
            IQueryable<object> data;
            try
            {
                var chaeckAdmin = await (from a in _context.Users
                                         where userId == a.userId
                                         select a).FirstOrDefaultAsync();
                if (chaeckAdmin.isAdmin == 1)
                {
                    data = (from a in _context.importHeaders
                            join b in _context.GradeImportHeaders on a.importHeaderNumber equals b.importHeaderNumber
                            where (string.IsNullOrEmpty(CourseName) ? true : a.courseName.Contains(CourseName))
                               && (string.IsNullOrEmpty(CourseID) ? true : a.courseID.Contains(CourseID))
                               && (string.IsNullOrEmpty(YearEducation) ? true : a.yearEducation.Contains(YearEducation))
                               && (string.IsNullOrEmpty(Semester) ? true : a.semester.Contains(Semester))
                            select new
                            {
                                a.importHeaderNumber,
                                a.courseID,
                                a.courseName,
                                a.semester,
                                b.Total,
                                b.A,
                                b.Bplus,
                                b.B,
                                b.Cplus,
                                b.C,
                                b.Dplus,
                                b.D,
                                b.F,
                                b.I,
                                b.W,
                                dateCreated = _timezoneConverter.ConvertToDefaultTimeZone((DateTime)(b.dateUpdated.HasValue ? b.dateUpdated : b.dateCreated), TimeZoneInfo.Utc),
                            });
                }
                else
                {
                    data = (from a in _context.importHeaders
                            join b in _context.GradeImportHeaders on a.importHeaderNumber equals b.importHeaderNumber
                            where (string.IsNullOrEmpty(CourseName) ? true : a.courseName.Contains(CourseName))
                               && (string.IsNullOrEmpty(CourseID) ? true : a.courseID.Contains(CourseID))
                               && (string.IsNullOrEmpty(YearEducation) ? true : a.yearEducation.Contains(YearEducation))
                               && (string.IsNullOrEmpty(Semester) ? true : a.semester.Contains(Semester))
                            where b.createByUserId == userId
                            select new
                            {
                                a.importHeaderNumber,
                                a.courseID,
                                a.courseName,
                                a.semester,
                                b.Total,
                                b.A,
                                b.Bplus,
                                b.B,
                                b.Cplus,
                                b.C,
                                b.Dplus,
                                b.D,
                                b.F,
                                b.I,
                                b.W,
                                dateCreated = _timezoneConverter.ConvertToDefaultTimeZone((DateTime)(b.dateUpdated.HasValue ? b.dateUpdated : b.dateCreated), TimeZoneInfo.Utc),
                            });
                }
                var result = await data.ToArrayAsync();

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
    }
}
