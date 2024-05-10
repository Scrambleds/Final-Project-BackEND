using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Service
{
    public interface IHomeService
    {
        List<ModelGrade> GetDataDashboard(string? CourseName, string? CourseID, string? YearEducation, string? Semester, int userId);
        JsonResult GetYearEducationList(int userId);
        JsonResult FilterSercYear(string? yearEducation, int userid);
        JsonResult FilterSercSemester(string? yearEducation, string? semester, int userid);
        JsonResult GetDataTable();
        JsonResult GetDataTableByUserId(string? CourseName, string? CourseID, string? YearEducation, string? Semester, int userId);
        byte[] GenerateHomeExcelFile(List<CourseInformation> itemList);
    }
}
