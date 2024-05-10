using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Repositorys
{
    public interface IHomeRepository
    {
        Task<ActionResult<List<ModelGrade>>> GetDataDashboard(string? CourseName, string? CourseID, string? YearEducation, string? Semester, int userId);
        Task<JsonResult> GetYearEducationList(int userId);
        Task<JsonResult> FilterSearchSemester(string? yearEducation, string? semester, int userid);
        Task<JsonResult> FilterSearchYear(string? yearEducation, int userid);
        Task<JsonResult> GetDataTable();
        Task<JsonResult> GetDataTableByUserId(string? CourseName, string? CourseID, string? YearEducation, string? Semester, int userId);

    }
}
