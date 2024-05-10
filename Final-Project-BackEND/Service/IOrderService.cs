using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Service
{
    public interface IOrderService
    {
        JsonResult GetOrderList();
        JsonResult GetOrder(int id);
        JsonResult SaveOrder(OrderVendor order);
        JsonResult UpdateOrder(OrderVendor order);
        JsonResult DeleteOrder(int id);
    }
}
