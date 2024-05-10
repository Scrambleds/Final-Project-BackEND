using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Final_Project_BackEND.Controllers
{
    public record ImportModel(List<Excel> ExcelData, ImportHeader ImportHeader, GradeImportHeader SumGrade);

    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private IImportService _importService;
        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost("CheckImportHeader")]
        public ActionResult CheckImportHeader(CheckImportHeader importHeader)
        {
            if (importHeader == null)
            {
                return BadRequest(new {status = -1 });
            }

            var result = _importService.CheckImportHeader(importHeader).Result;
            return result;
        }

        [HttpGet("GetListExcelFromImportHeader/{importHeaderNumber}")]
        public ActionResult GetListExcelFromImportHeader(string importHeaderNumber)
        {
            if (importHeaderNumber.IsNullOrEmpty())
            {
                return NotFound();
            }

            var listExcel = _importService.GetListExcelFromImportHeader(importHeaderNumber).Result;
            return listExcel;
        }

        [HttpPost]
        public async Task<ActionResult> SaveScoreByImportAsync([FromBody] ImportModel importModel)
        {
            if (importModel == null || importModel.ExcelData == null || importModel.ImportHeader == null || importModel.SumGrade == null)
            {
                return BadRequest("Invalid Input");
            }

            var importResult = await _importService.SaveScoreByImportAsync(importModel.ExcelData, importModel.ImportHeader, importModel.SumGrade);

            //if (importResult == -1)
            //{
            //    return BadRequest("บันทึกไม่สำเร็จ");
            //}

            return Ok(importResult);
        }
    }
}
