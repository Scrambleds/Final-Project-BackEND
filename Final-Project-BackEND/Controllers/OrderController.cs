using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Repositorys;
using Final_Project_BackEND.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Final_Project_BackEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        [HttpGet("GetOrderList")]
        public IActionResult GetOrderList()
        {
            var data = _orderService.GetOrderList().Value;
            return Ok(data);
        }

        [HttpGet("GetOrder/{id}")]
        public IActionResult GetOrder(int id)
        {            
            var data = _orderService.GetOrder(id).Value;
            return Ok(data);
        }

        [HttpPost("SaveOrder")]
        public IActionResult SaveOrder(OrderVendor order)
        {
            var data = _orderService.SaveOrder(order).Value;
            return Ok(data);
        }

        [HttpPost("UpdateOrder")]
        public ActionResult UpdateOrder(OrderVendor order)
        {
            var data = _orderService.UpdateOrder(order).Value;
            return Ok(data);
        }

        [HttpDelete("DeleteOrder/{id}")]
        public ActionResult DeleteOrder(int id)
        {
            var data = _orderService.DeleteOrder(id).Value;
            return Ok(data);
        }

        //[HttpGet("TestJoin/{importHeaderNumber}")]
        //public ActionResult TestJoin(string importHeaderNumber)
        //{
        //    var data = (from a in _context.importHeaders
        //                join b in _context.Excels
        //                on a.importHeaderNumber equals b.importHeaderNumber
        //                where a.importHeaderNumber == importHeaderNumber
        //                select new
        //                {
        //                    a.importHeaderNumber,
        //                    a.courseName,
        //                    a.courseID,
        //                    b.name,
        //                    b.grade
        //                }).ToList();


        //    if (data == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(data);
        //}
    }
}
