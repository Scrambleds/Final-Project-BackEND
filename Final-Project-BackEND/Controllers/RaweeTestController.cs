using Final_Project_BackEND.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaweeTestController : ControllerBase
    {
        private readonly DataContext _context;
        public RaweeTestController(DataContext db) 
        {
            _context = db;
        }
        [HttpGet("GetRaweeList")]
        public ActionResult GetRaweeList() 
        {
            //LINQ แบบเต็ม 
            var data = (from a in _context.RaweeTest
                        where a.rawee_name == "RaweeKU81"
                        select a).ToList();

            //LINQ แบบสั้น
            //var result = _context.OrderVendors.ToList();

            //Execute query
            /*string sqlCon = "SELECT * FROM RaweeTest";

            var result = _context.ExecuteQuery(sqlCon, new SqlParameter());*/
            
            if(data == null) 
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
}