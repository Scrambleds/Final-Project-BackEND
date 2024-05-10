using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Repositorys
{
    public interface IImportRepository
    {
        Task<ActionResult> CheckImportHeader(CheckImportHeader importHeader);
        Task<ActionResult> GetListExcelFromImportHeader(string importHeaderNumber);
        Task<ActionResult> AddExcel(List<Excel> newExcel);
        Task<int> SaveScoreByImportAsync(List<Excel> excelList, ImportHeader ImportH, GradeImportHeader SumGrade);
        //string GenerateInvoiceNumber();
    }
}
