using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Repositorys;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

namespace Final_Project_BackEND.Service
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;
        private readonly DataContext _context;
        private readonly TimezoneConverter _timezoneConverter;

        public HomeService(IHomeRepository homeRepository, DataContext db, TimezoneConverter timezoneConverter)
        {
            _homeRepository = homeRepository;
            _context = db;
            _timezoneConverter = timezoneConverter;
        }

        public List<ModelGrade> GetDataDashboard(string? CourseName, string? CourseID, string? YearEducation, string? Semester,int userId)
        {
            var data = _homeRepository.GetDataDashboard(CourseName, CourseID, YearEducation, Semester, userId).Result.Value;
            return data ?? new List<ModelGrade>();
        }
        public JsonResult GetYearEducationList(int userId)
        {
            var data = _homeRepository.GetYearEducationList(userId).Result;
            return data;
        }

        public JsonResult FilterSercYear(string? yearEducation, int userid)
        {
            var data = _homeRepository.FilterSearchYear(yearEducation, userid).Result;
            return data;
        }
        public JsonResult FilterSercSemester(string? yearEducation, string? semester, int userid)
        {
            var data = _homeRepository.FilterSearchSemester(yearEducation,semester,userid).Result;
            return data;
        }
        public JsonResult GetDataTable()
        {
            var data = _homeRepository.GetDataTable().Result;
            return data;
        }

        public JsonResult GetDataTableByUserId(string? CourseName, string? CourseID, string? YearEducation, string? Semester, int userId)
        {
            var data = _homeRepository.GetDataTableByUserId(CourseName, CourseID, YearEducation, Semester, userId).Result;
            return data;
        }
        public byte[] GenerateHomeExcelFile(List<CourseInformation> itemList)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("StudentDeatail");

                ExcelRange headerRange = worksheet.Cells[1, 1, 1, 17];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells[1, 1].Value = "No";
                worksheet.Cells[1, 2].Value = "หมายเลขอัปโหลด";
                worksheet.Cells[1, 3].Value = "รหัสวิชา";
                worksheet.Cells[1, 4].Value = "วิชา";
                worksheet.Cells[1, 5].Value = "ภาคการศึกษา";
                worksheet.Cells[1, 6].Value = "จำนวนนิสิต";
                worksheet.Cells[1, 7].Value = "อัพโหลดวันที่";
                worksheet.Cells[1, 8].Value = "A";
                worksheet.Cells[1, 9].Value = "B+";
                worksheet.Cells[1, 10].Value = "B";
                worksheet.Cells[1, 11].Value = "C+";
                worksheet.Cells[1, 12].Value = "C";
                worksheet.Cells[1, 13].Value = "D+";
                worksheet.Cells[1, 14].Value = "D";
                worksheet.Cells[1, 15].Value = "I";
                worksheet.Cells[1, 16].Value = "F";
                worksheet.Cells[1, 17].Value = "W";

                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Column(1).Width = 5; // Set the width of column A
                worksheet.Column(2).Width = 25; // Set the width of column B
                worksheet.Column(3).Width = 20; // Set the width of column C
                worksheet.Column(4).Width = 30; // Set the width of column D
                worksheet.Column(5).Width = 15; // Set the width of column E
                worksheet.Column(6).Width = 15; // Set the width of column F
                worksheet.Column(7).Width = 15; // Set the width of column G

                int row = 2;
                int no = 1;
                worksheet.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                foreach (var item in itemList)
                {
                    worksheet.Cells[row, 1].Value = no++;
                    worksheet.Cells[row, 2].Value = item.importHeaderNumber;
                    worksheet.Cells[row, 3].Value = item.courseID;
                    worksheet.Cells[row, 4].Value = item.courseName;
                    worksheet.Cells[row, 5].Value = item.semester;
                    worksheet.Cells[row, 6].Value = item.total;
                    worksheet.Cells[row, 7].Value = item.dateCreated.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 8].Value = item.a;
                    worksheet.Cells[row, 9].Value = item.bplus;
                    worksheet.Cells[row, 10].Value = item.b;
                    worksheet.Cells[row, 11].Value = item.cplus;
                    worksheet.Cells[row, 12].Value = item.c;
                    worksheet.Cells[row, 13].Value = item.dplus;
                    worksheet.Cells[row, 14].Value = item.d;
                    worksheet.Cells[row, 15].Value = item.f;
                    worksheet.Cells[row, 16].Value = item.i;
                    worksheet.Cells[row, 17].Value = item.w;

                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream.ToArray();
            }
        }
    }
}
