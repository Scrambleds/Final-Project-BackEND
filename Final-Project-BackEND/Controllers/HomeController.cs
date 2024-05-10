using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Final_Project_BackEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        [HttpGet("GetDataDashboard")]
        public IActionResult GetDataDashboard(string? CourseName,string? CourseID,string? YearEducation,string? Semester)
        {
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    var result = _homeService.GetDataDashboard(CourseName, CourseID, YearEducation, Semester, userId);

                    if (result == null)
                    {
                        return NotFound(new { result = "ไม่พบข้อมูลสำหรับ userId ที่ระบุ" });
                    }

                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { result = "เกิดข้อผิดพลาดในการแปลง userId เป็นตัวเลข" });
                }
            }

            return BadRequest(new { result = "ไม่พบ userId ใน Header" });
            
        }
        [HttpGet("GetYearEducationList")]
        public IActionResult GetYearEducationList()
        {
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    var result = _homeService.GetYearEducationList(userId).Value;

                    if (result == null)
                    {
                        return NotFound(new { result = "ไม่พบข้อมูลสำหรับ userId ที่ระบุ" });
                    }

                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { result = "เกิดข้อผิดพลาดในการแปลง userId เป็นตัวเลข" });
                }
            }

            return BadRequest(new { result = "ไม่พบ userId ใน Header" });
        }

        [HttpGet("FilterSearchYear")]
        public IActionResult FilterSearchYear(string? yearEducation)
        {
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    var result = _homeService.FilterSercYear(yearEducation, userId).Value;

                    if (result == null)
                    {
                        return NotFound(new { result = "ไม่พบข้อมูลสำหรับ userId ที่ระบุ" });
                    }

                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { result = "เกิดข้อผิดพลาดในการแปลง userId เป็นตัวเลข" });
                }
            }
            return BadRequest(new { result = "ไม่พบ userId ใน Header" });
        }

        [HttpGet("FilterSearchSemester")]
        public IActionResult FilterSearchSemester(string? yearEducation, string? semester)
        {
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    var result = _homeService.FilterSercSemester(yearEducation, semester, userId).Value;

                    if (result == null)
                    {
                        return NotFound(new { result = "ไม่พบข้อมูลสำหรับ userId ที่ระบุ" });
                    }

                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { result = "เกิดข้อผิดพลาดในการแปลง userId เป็นตัวเลข" });
                }
            }
            return BadRequest(new { result = "ไม่พบ userId ใน Header" });
        }

        [HttpGet("GetDataTable")]
        public IActionResult GetDataTable()
        {
            var result = _homeService.GetDataTable().Value;

            if (result == null)
            {
                return NotFound(new { result = "เกิดข้อผิดพลาดบางอย่าง" });
            }

            return Ok(result);
        }

        [HttpGet("GetDataTableByUserId")]
        public IActionResult GetDataTableByUserId(string? CourseName, string? CourseID, string? YearEducation, string? Semester)
        {
            if (Request.Headers.TryGetValue("userId", out var userIdHeader))
            {
                if (int.TryParse(userIdHeader, out int userId))
                {
                    var data = _homeService.GetDataTableByUserId(CourseName, CourseID, YearEducation, Semester, userId).Value;

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
        
        [HttpPost("GenerateHomeExcel")]
        public IActionResult GenerateHomeExcel(List<CourseInformation> itemList)
        {
            byte[] excelBytes = _homeService.GenerateHomeExcelFile(itemList);
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ข้อมูลรายนิสิต" + ".xlsx");

        }
    }
}
