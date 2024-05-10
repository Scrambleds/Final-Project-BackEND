using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Final_Project_BackEND.Service
{
    public class ListService : IListService
    {
        private readonly IListRepository _listRepository;
        private readonly DataContext _context;

        public ListService(IListRepository listRepository, DataContext db)
        {
            _listRepository = listRepository;
            _context = db;
        }

        public async Task<List<ImportHeader>> GetAlllistimportheader()
        {
            return await _listRepository.GetAlllistimportheader();
        }

        public async Task<List<ImportHeader>> Getlistimportheader(string? ImportHeaderNo, string? CourseID, string? YearEducation, string? page, int userId)
        {
            var alllist = await _listRepository.Getlistimportheader(ImportHeaderNo,CourseID, YearEducation,page, userId);
            return alllist;
        }

        public async Task<List<ImportHeader>> GetlistimportheaderForPage(string page, int userId)
        {
            return await _listRepository.GetlistimportheaderForPage(page, userId);
        }

        public async Task<int> CountListimportheader(string? importNo, string? courseID, int userId)
        {
            return await _listRepository.CountListimportheader(importNo,courseID, userId);
        }

        public async Task<List<ImportHeader>> GetlistimportheaderByFilter(importHSearch filter)
        {
            var alllist = await _listRepository.GetlistimportheaderByFilter(filter);
            return alllist;
        }

        public int DeleteImportList(string importHederNumber)
        {
            return _listRepository.DeleteImportList(importHederNumber).Result;
        }

        public ImportHeader GetDetailByHeaderNo(string importHeaderNumber)
        {
            return _listRepository.GetDetailByHeaderNo(importHeaderNumber).Result;
        }

        public async Task<List<ImportHeader>> GetDetailByHeaderNoByUserId(string importHeaderNumber, int userId)
        {
            return await _listRepository.GetDetailByHeaderNoByUserId(importHeaderNumber, userId);
        }

        public JsonResult GetExcelDetail(string importHeaderNumber)
        {
            var data = _listRepository.GetExcelDetail(importHeaderNumber).Result;
            return data;
        }

        public JsonResult GetExcelDetailByHeaderNoByUserId(string importHeaderNumber, int userId)
        {
            var data =  _listRepository.GetExcelDetailByHeaderNoByUserId(importHeaderNumber, userId).Result; 
            return data;
        }

        public async Task<List<GroupStudentInExcel>> GetAllStudent()
        {
            return await _listRepository.GetAllStudent();
         }


        public async Task<List<GroupStudentInExcel>> GetAllStudentForPage(string? id, string? name, string? page, int userId)
        {
            return await _listRepository.GetAllStudentForPage(id,name,page,userId);
        }

        public async Task<int> CountAllStudent(string? id, string? name, int userId)
        {
            return await _listRepository.CountAllStudent(id, name, userId);
        }

        public async Task<List<GradeFromId>> GetGradeStudent(string? id)
        {
            return await _listRepository.GetGradeStudent(id);
        }

        public async Task<List<GradeFromId>> GetGradeStudentByUserId(string? id, int userId)
        {
            return await _listRepository.GetGradeStudentByUserId(id, userId);
        }

        public JsonResult SumGradeStudent(string? id)
        {
            var data = _listRepository.SumGradeStudent(id).Result;
            return data;
        }

        public JsonResult SumGradeStudentByUserId(string? id, int userId)
        {
            var data = _listRepository.SumGradeStudentByUserId(id,userId).Result;
            return data;
        }

        public async Task<List<GradeImportHeader>> GetGradeImportHeaderNumber(string? importHeaderNumber)
        {
            return await _listRepository.GetGradeImportHeaderNumber(importHeaderNumber);
        }
        public async Task<List<GradeImportHeader>> GetGradeImportHeaderNumberByUserId(string? importHeaderNumber, int userId)
        {
            return await _listRepository.GetGradeImportHeaderNumberByUserId(importHeaderNumber, userId);
        }

        public JsonResult GetDropdownYearEducation(int userId)
        {
            var data = _listRepository.GetDropdownYearEducation(userId).Result;
            return data;
        }

        public byte[] GenerateExcelFile(List<StudentExcel> students,List<GradeImportHeader> grade)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Students");

                // Add headers with styles
                ExcelRange headerRange = worksheet.Cells[1, 1, 1, 4];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                // Set header values directly using string indices
                worksheet.Cells[1, 1].Value = "No";
                worksheet.Cells[1, 2].Value = "ID";
                worksheet.Cells[1, 3].Value = "Name";
                worksheet.Cells[1, 4].Value = "Grade";

                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Add data
                for (int i = 0; i < students.Count; i++)
                {
                    var student = students[i];
                    worksheet.Cells[i + 2, 1].Value = student.No;
                    worksheet.Cells[i + 2, 2].Value = student.Id;
                    worksheet.Cells[i + 2, 3].Value = student.Name;
                    worksheet.Cells[i + 2, 4].Value = student.Grade;

                    worksheet.Cells[i + 2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[i + 2, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Set column width (example: setting width of columns A, B, C, and D)
                worksheet.Column(1).Width = 5; // Set the width of column A
                worksheet.Column(2).Width = 15; // Set the width of column B
                worksheet.Column(3).Width = 30; // Set the width of column C
                worksheet.Column(4).Width = 8; // Set the width of column D

                worksheet.Cells[1, 8].Value = "A";
                worksheet.Cells[1, 9].Value = "B+";
                worksheet.Cells[1, 10].Value = "B";
                worksheet.Cells[1, 11].Value = "C+";
                worksheet.Cells[1, 12].Value = "C";
                worksheet.Cells[1, 13].Value = "D+";
                worksheet.Cells[1, 14].Value = "D";
                worksheet.Cells[1, 15].Value = "F";
                worksheet.Cells[1, 16].Value = "I";
                worksheet.Cells[1, 17].Value = "W";
                worksheet.Cells[1, 18].Value = "รวม";

                ExcelRange footerHeaderRange = worksheet.Cells[1, 7, 1, 18];
                footerHeaderRange.Style.Font.Bold = true;
                footerHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                footerHeaderRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                footerHeaderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells[2, 7].Value = "Total:";
                worksheet.Cells[2, 8].Value = grade[0].A ?? 0;
                worksheet.Cells[2, 9].Value = grade[0].Bplus ?? 0;
                worksheet.Cells[2, 10].Value = grade[0].B ?? 0;
                worksheet.Cells[2, 11].Value = grade[0].Cplus ?? 0;
                worksheet.Cells[2, 12].Value = grade[0].C ?? 0;
                worksheet.Cells[2, 13].Value = grade[0].Dplus ?? 0;
                worksheet.Cells[2, 14].Value = grade[0].D ?? 0;
                worksheet.Cells[2, 15].Value = grade[0].F ?? 0;
                worksheet.Cells[2, 16].Value = grade[0].I ?? 0;
                worksheet.Cells[2, 17].Value = grade[0].W ?? 0;
                worksheet.Cells[2, 18].Value = grade[0].Total ?? 0;
                // Add other footer data fields as needed

                ExcelRange footerdataRange = worksheet.Cells[2, 8,2, 18];
                footerdataRange.Style.Font.Bold = true;
                footerdataRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                footerdataRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                footerdataRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Style the footer
                ExcelRange footerRange = worksheet.Cells[2, 7];
                footerRange.Style.Font.Bold = true;
                footerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                footerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                footerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Save the Excel file to a MemoryStream
                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream.ToArray();
            }
        }

        public byte[] GenerateStudentDetailExcelFile(List<StudentDetailExcel> studentsDetail, GradeSumStudent grade)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("StudentDeatail");

                ExcelRange headerRange = worksheet.Cells[1, 1, 1, 8];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells[1, 1].Value = "No";
                worksheet.Cells[1, 2].Value = "ID";
                worksheet.Cells[1, 3].Value = "Name";
                worksheet.Cells[1, 4].Value = "CourseID";
                worksheet.Cells[1, 5].Value = "CourseName";
                worksheet.Cells[1, 6].Value = "Semester";
                worksheet.Cells[1, 7].Value = "YearEducation";
                worksheet.Cells[1, 8].Value = "Grade";

                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                for (int i = 0; i < studentsDetail.Count; i++)
                {
                    var student = studentsDetail[i];
                    worksheet.Cells[i + 2, 1].Value = i+1;
                    worksheet.Cells[i + 2, 2].Value = student.id;
                    worksheet.Cells[i + 2, 3].Value = student.name;
                    worksheet.Cells[i + 2, 4].Value = student.courseID;
                    worksheet.Cells[i + 2, 5].Value = student.courseName;
                    worksheet.Cells[i + 2, 6].Value = student.semester;
                    worksheet.Cells[i + 2, 7].Value = student.yearEducation;
                    worksheet.Cells[i + 2, 8].Value = student.grade;

                    worksheet.Cells[i + 2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[i + 2, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[i + 2, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                }

                worksheet.Column(1).Width = 5; // Set the width of column A
                worksheet.Column(2).Width = 15; // Set the width of column B
                worksheet.Column(3).Width = 30; // Set the width of column C
                worksheet.Column(4).Width = 15; // Set the width of column D
                worksheet.Column(5).Width = 30; // Set the width of column E
                worksheet.Column(6).Width = 15; // Set the width of column F
                worksheet.Column(7).Width = 15; // Set the width of column G
                worksheet.Column(8).Width = 15; // Set the width of column H

                worksheet.Cells[1, 12].Value = "A";
                worksheet.Cells[1, 13].Value = "B+";
                worksheet.Cells[1, 14].Value = "B";
                worksheet.Cells[1, 15].Value = "C+";
                worksheet.Cells[1, 16].Value = "C";
                worksheet.Cells[1, 17].Value = "D+";
                worksheet.Cells[1, 18].Value = "D";
                worksheet.Cells[1, 19].Value = "F";
                worksheet.Cells[1, 20].Value = "I";
                worksheet.Cells[1, 21].Value = "W";
                worksheet.Cells[1, 22].Value = "รวม";

                ExcelRange footerHeaderRange = worksheet.Cells[1, 11, 1, 22];
                footerHeaderRange.Style.Font.Bold = true;
                footerHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                footerHeaderRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                footerHeaderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells[2, 11].Value = "Total:";
                worksheet.Cells[2, 12].Value = grade.a ?? "0";
                worksheet.Cells[2, 13].Value = grade.bPlus ?? "0";
                worksheet.Cells[2, 14].Value = grade.b ?? "0";
                worksheet.Cells[2, 15].Value = grade.cPlus ?? "0";
                worksheet.Cells[2, 16].Value = grade.c ?? "0";
                worksheet.Cells[2, 17].Value = grade.dPlus ?? "0";
                worksheet.Cells[2, 18].Value = grade.d ?? "0";
                worksheet.Cells[2, 19].Value = grade.f ?? "0";
                worksheet.Cells[2, 20].Value = grade.i ?? "0";
                worksheet.Cells[2, 21].Value = grade.w ?? "0";
                worksheet.Cells[2, 22].Value = grade.total ?? "0";

                ExcelRange footerdataRange = worksheet.Cells[2, 11, 2, 22];
                footerdataRange.Style.Font.Bold = true;
                footerdataRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                footerdataRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                footerdataRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ExcelRange footerRange = worksheet.Cells[2, 11];
                footerRange.Style.Font.Bold = true;
                footerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                footerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                footerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream.ToArray();
            }

        }
    }
}
