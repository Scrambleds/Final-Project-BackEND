using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using Final_Project_BackEND.Migrations;
using static Final_Project_BackEND.Controllers.ListController;
using GradeImportHeader = Final_Project_BackEND.Entity.GradeImportHeader;

namespace Final_Project_BackEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private IListService _listService;
        public ListController(IListService listService)
        {
            _listService = listService;
        }

        [HttpGet("GetAlllistimportheader")]
        public ActionResult<List<ImportHeader>> GetAlllistimportheader()
        {
            var result = _listService.GetAlllistimportheader().Result;
            return Ok(result);
        }

        [HttpGet("Getlistimportheader")]
        public ActionResult<List<ImportHeader>> Getlistimportheader(string? ImportHeaderNo, string? CourseID,string? YearEducation, string? page)
        {
            List<ImportHeader> data = new List<ImportHeader>();
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    data = _listService.Getlistimportheader(ImportHeaderNo, CourseID, YearEducation, page,userId).Result;
                }
                else
                {
                    return BadRequest("Invalid userId format");
                }
            }
              
            return Ok(data);
        }

        [HttpGet("GetlistimportheaderForPage")]
        public async Task<IActionResult> GetlistimportheaderForPage(string? page)
        {
            List<ImportHeader> data = new List<ImportHeader>();

            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    data = _listService.GetlistimportheaderForPage(page, userId).Result;
                }
                else
                {
                    return BadRequest("Invalid userId format");
                }
            }
            else
            {
                return NotFound("userId header not present in the request");
            }

            return Ok(data);
        }


        [HttpGet("CountListimportheader")]
        public async Task<ActionResult<int>> CountListimportheader(string? ImportHeaderNo, string? CourseID)
        {
            int count = 0;

            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    count = await _listService.CountListimportheader(ImportHeaderNo, CourseID, userId);
                }
                else
                {
                    count = 0;
                }
            }
            else
            {
                count = 0;
            }

            return count;
        }


        [HttpGet("GetlistimportheaderByFilter")]
        public ActionResult<IEnumerable<ImportHeader>> GetlistimportheaderByFilter([FromQuery] importHSearch filter)
        {
            try
            {
                var data = _listService.GetlistimportheaderByFilter(filter).Result;
                if (data != null)
                {
                    return Ok(data);
                }
                else
                {
                    // Handle the case when data is null, return appropriate response
                    return NotFound("No data found for the given filter");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetDetailByHeaderNo/{importHeaderNumber}")]
        public ActionResult GetDetailByHeaderNo(string importHeaderNumber)
        {
            var result = _listService.GetDetailByHeaderNo(importHeaderNumber);
            return Ok(result);
        }

        [HttpGet("GetDetailByHeaderNoByUserId/{importHeaderNumber}")]
        public async Task<IActionResult> GetDetailByHeaderNoByUserId(string importHeaderNumber)
        {

            List<ImportHeader> data = new List<ImportHeader>();
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    data = _listService.GetDetailByHeaderNoByUserId(importHeaderNumber, userId).Result;
                }
                else
                {
                    return BadRequest("Invalid userId format");
                }
            }

            return Ok(data);
        }

        [HttpGet("GetExcelDetailByHeaderNo/{importHeaderNumber}")]
        public IActionResult GetExcelDetail(string importHeaderNumber)
        {
                var result = _listService.GetExcelDetail(importHeaderNumber).Value;
                if (result == null)
                {
                    return NotFound(new { result = "ไม่พบข้อมูล" });
                }
                else
                {
                    return Ok(result);

                }
        }

        [HttpGet("GetExcelDetailByHeaderNoByUserId/{importHeaderNumber}")]
        public IActionResult GetExcelDetailByHeaderNoByUserId(string importHeaderNumber)
        {
           
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    var data = _listService.GetExcelDetailByHeaderNoByUserId(importHeaderNumber, userId).Value;
                    if (data == null)
                    {
                        return NotFound(new { result = "ไม่พบข้อมูลสำหรับ userId ที่ระบุ" });
                    }
                    return Ok(data);
                }
                else
                {
                    return BadRequest(new { result = "เกิดข้อผิดพลาดในการแปลง userId เป็นตัวเลข" });
                }
            }

            return BadRequest(new { result = "ไม่พบ userId ใน Header" });
        }


        [HttpDelete("DeleteImportList")]
        public ActionResult DeleteImportList(string importHedderNumber)
        {
            var result = _listService.DeleteImportList(importHedderNumber);
            return Ok(result);
        }

        [HttpGet("GetAllStudent")]
        public async Task<IActionResult> GetAllStudent()
        {
            var result = _listService.GetAllStudent().Result;
            if (result.Count == 0)
            {
                return BadRequest("There are no students in the system.");
            }
            return Ok(result);
        }


        [HttpGet("GetAllStudentForPage")]
        public async Task<IActionResult> GetAllStudentForPage(string? id, string? name, string? page)
        {
            List<GroupStudentInExcel> data = new List<GroupStudentInExcel>();
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    data = _listService.GetAllStudentForPage(id,name,page,userId).Result;
                }
                else
                {
                    return BadRequest("There are no students in the system.");
                }
            }
            return Ok(data);
        }

        [HttpGet("CountAllStudent")]
        public async Task<ActionResult<int>> CountAllStudent(string? id, string? name)
        {
            int count = 0;

            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    count = await _listService.CountAllStudent(id, name, userId);
                }
                else
                {
                    count = 0;
                }
            }
            else
            {
                count = 0;
            }

            return count;
        }

        [HttpGet("GetGradeStudent/{id}")]
        public async Task<IActionResult> GetGradeStudent(string? id)
        {
            var result = _listService.GetGradeStudent(id).Result;
            if (result.Count == 0)
            {
                return BadRequest("There are no students in the system.");
            }
            return Ok(result);
        }

        [HttpGet("GetGradeStudentByUserId/{id}")]
        public async Task<IActionResult> GetGradeStudentByUserId(string? id)
        {
            List<GradeFromId> data = new List<GradeFromId>();
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    data = _listService.GetGradeStudentByUserId(id, userId).Result;
                }
                else
                {
                    return BadRequest("There are no students in the system.");
                }
            }
            return Ok(data);
        }

        [HttpGet("SumGradeStudent/{id}")]
        public IActionResult SumGradeStudent(string? id)
        {
            var result = _listService.SumGradeStudent(id).Value;
            if (result == null)
            {
                return NotFound(new { result = "ไม่พบข้อมูล" });
            }
            else
            {
                return Ok(result);

            }
        }

        [HttpGet("SumGradeStudentByUserId/{id}")]
        public IActionResult SumGradeStudentByUserId(string? id)
        {
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                   var data = _listService.SumGradeStudentByUserId(id, userId).Value;

                    if (data == null)
                    {
                        return NotFound(new { result = "ไม่พบข้อมูลสำหรับ userId ที่ระบุ" });
                    }

                    return Ok(data);
                }
                else
                {
                    return BadRequest(new { result = "เกิดข้อผิดพลาดในการแปลง userId เป็นตัวเลข" });
                }
            }
            return BadRequest(new { result = "ไม่พบ userId ใน Header" });
        }


        [HttpGet("GetGradeImportHeaderNumber/{importHeaderNumber}")]
        public async Task<IActionResult> GetGradeImportHeaderNumber(string? importHeaderNumber)
        {
            var result = _listService.GetGradeImportHeaderNumber(importHeaderNumber).Result;
            if (result == null)
            {
                return BadRequest("There are no students in the system.");
            }
            return Ok(result);
        }

        [HttpGet("GetGradeImportHeaderNumberByUserId/{importHeaderNumber}")]
        public async Task<IActionResult> GetGradeImportHeaderNumberByUserId(string? importHeaderNumber)
        {
            List<Entity.GradeImportHeader> data = new List<Entity.GradeImportHeader>();

            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    data = _listService.GetGradeImportHeaderNumberByUserId(importHeaderNumber, userId).Result;
                }
                else
                {
                    return BadRequest("There are no students in the system.");
                }
            }
            return Ok(data);
        }

        public record ExportModel(List<StudentExcel> students, List<GradeImportHeader> grade, string? id);
        [HttpPost("GenerateExcel")]
        public IActionResult GenerateExcel([FromBody] ExportModel exportModel)
        {
            // Process the list of students and generate Excel
            byte[] excelBytes = _listService.GenerateExcelFile(exportModel.students,exportModel.grade);

            // Return the Excel file to the client
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ข้อมูลรายวิชา "+ exportModel.id + ".xlsx");
        }

        public record ExportStudentDetailModel(List<StudentDetailExcel> studentsDetail, GradeSumStudent grade,string? id);
        [HttpPost("GenerateStudentDetailExcel")]
        public IActionResult GenerateStudentDetailExcel([FromBody] ExportStudentDetailModel exportModel)
        {
            byte[] excelBytes = _listService.GenerateStudentDetailExcelFile(exportModel.studentsDetail, exportModel.grade);
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ข้อมูลรายนิสิต" + exportModel.id + ".xlsx");
        }

        [HttpGet("GetDropdownYearEducation")]
        public IActionResult GetDropdownYearEducation()
        {
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    var data = _listService.GetDropdownYearEducation(userId).Value;

                    if (data == null)
                    {
                        return NotFound(new { result = "ไม่พบข้อมูลสำหรับ userId ที่ระบุ" });
                    }

                    return Ok(data);
                }
                else
                {
                    return BadRequest(new { result = "เกิดข้อผิดพลาดในการแปลง userId เป็นตัวเลข" });
                }
            }
            return BadRequest(new { result = "ไม่พบ userId ใน Header" });
        }

    }
}
