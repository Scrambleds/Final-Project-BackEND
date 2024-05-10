using Final_Project_BackEND.Controllers;
using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Service
{
    public interface IImportService
    {
        Task<ActionResult> CheckImportHeader(CheckImportHeader importHeader);
        Task<ActionResult> GetListExcelFromImportHeader(string importHeaderNumber);
        Task<int> SaveScoreByImportAsync(List<Excel> excelList, ImportHeader ImportH, GradeImportHeader SumGrade);
    }
}
