using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Service
{
    public interface IListService
    {
        Task<List<ImportHeader>> GetAlllistimportheader();
        Task<List<ImportHeader>> Getlistimportheader(string? ImportHeaderNo, string? CourseID, string? YearEducation, string? page, int userId);
        Task<List<ImportHeader>> GetlistimportheaderForPage(string page,int userId);
        Task<int> CountListimportheader(string? importNo, string? courseID,int userId);
        int DeleteImportList(string importHederNumber);

        JsonResult GetDropdownYearEducation(int userId);
        Task<List<ImportHeader>> GetlistimportheaderByFilter(importHSearch filter);
        ImportHeader GetDetailByHeaderNo(string importHeaderNumber);
        Task<List<ImportHeader>> GetDetailByHeaderNoByUserId(string importHeaderNumber, int userId);
        JsonResult GetExcelDetail(string importHeaderNumber);
        JsonResult GetExcelDetailByHeaderNoByUserId(string importHeaderNumber, int userId);
        Task<List<GroupStudentInExcel>> GetAllStudent();
        Task<List<GroupStudentInExcel>> GetAllStudentForPage(string? id, string? name, string? page, int userId);
        Task<int> CountAllStudent(string? id, string? name, int userId);
        Task<List<GradeFromId>> GetGradeStudent(string? id);
        Task<List<GradeFromId>> GetGradeStudentByUserId(string? id, int userId);
        JsonResult SumGradeStudent(string? id);
        JsonResult SumGradeStudentByUserId(string? id, int userId);
        Task<List<GradeImportHeader>> GetGradeImportHeaderNumber(string? importHeaderNumber);
        Task<List<GradeImportHeader>> GetGradeImportHeaderNumberByUserId(string? importHeaderNumber, int userId);
        byte[] GenerateExcelFile(List<StudentExcel> students, List<GradeImportHeader> grade);
        byte[] GenerateStudentDetailExcelFile(List<StudentDetailExcel> studentsDetail, GradeSumStudent grade);
    }
}
