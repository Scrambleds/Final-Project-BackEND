using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_BackEND.Repositorys
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        public OrderRepository(DataContext db)
        {
            _context = db;
        }

        public async Task<JsonResult> GetOrderList()
        {
            var data = await (from a in _context.OrderVendors
                        select a).ToListAsync();

            //var sqlCom = "SELECT * FROM OrderVendors";
            //var result1 = _context.OrderVendors.FromSqlRaw(sqlCom).ToList();

            if (data == null)
            {
                return new JsonResult(new { error = "ไม่มีข้อมูลในระบบ" });
            }
            return new JsonResult(data);
        }

        public async Task<JsonResult> GetOrder(int id)
        {
            var data =  await (from a in _context.OrderVendors
                        where a.order_id == id
                        select a).FirstOrDefaultAsync();

            if (data == null)
            {
                return new JsonResult(new { error = "ไม่มีข้อมูลในระบบ" });
            }
            return new JsonResult(data);
        }

        public async Task<JsonResult> SaveOrder(OrderVendor order)
        {
            var checkname = await (from a in _context.OrderVendors
                                   where a.vendor_name == order.vendor_name
                                   select a).FirstOrDefaultAsync();

            if (checkname == null)
            {
                try
                {
                    var result = await _context.OrderVendors.AddAsync(order);

                    if (result != null)
                    {
                        _context.SaveChanges();
                        return new JsonResult(new { message = "บันทึกข้อมูลสำเร็จ" });
                    }
                    else
                    {
                        return new JsonResult(new { message = "ไม่สามารถบันทึกข้อมูลได้" });
                    }

                }
                catch (Exception ex)
                {
                    return new JsonResult(new { error = "เกิดข้อผิดพลาด" });
                }
            }
            else
            {
                return new JsonResult(new { error = "มีข้อมูลในระบบแล้ว" });
            }
        }

        public async Task<JsonResult> UpdateOrder(OrderVendor order)
        {
            var checkname = await (from a in _context.OrderVendors
                             where a.order_id == order.order_id
                             select a).FirstOrDefaultAsync();

            if (checkname != null)
            {
                //a = B
                checkname.vendor_name = order.vendor_name;

                try
                {
                    _context.SaveChanges();
                    return new JsonResult(checkname);
                }
                catch (Exception)
                {
                    return new JsonResult(new { error = "เกิดข้อผิดพลาด" });
                }
            }
            else
            {
                return new JsonResult(new { error = "ไม่มีข้อมูลในระบบ" });
            }
        }

        public async Task<JsonResult> DeleteOrder(int id)
        {
            var checkorder = await (from a in _context.OrderVendors
                              where a.order_id == id
                              select a).FirstOrDefaultAsync();

            if (checkorder != null)
            {
                try
                {
                    var result = _context.OrderVendors.Remove(checkorder);

                    if (result != null)
                    {
                        _context.SaveChanges();
                        return new JsonResult(new { message = "ลบข้อมูลสำเร็จ" });
                    }
                    else
                    {
                        return new JsonResult(new { error = "ไม่สามารถลบได้" });
                    }

                }
                catch (Exception)
                {
                    return new JsonResult(new { error = "เกิดข้อผิดพลาด" });
                }
            }
            else
            {
                return new JsonResult(new { error = "ไม่มีข้อมูลในระบบ" });
            }
        }
    }
}
