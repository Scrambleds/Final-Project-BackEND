using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Repositorys
{
    public interface IOrderRepository
    {
        Task<JsonResult> GetOrderList();
        Task<JsonResult> GetOrder(int id);
        Task<JsonResult> SaveOrder(OrderVendor order);
        Task<JsonResult> UpdateOrder(OrderVendor order);
        Task<JsonResult> DeleteOrder(int id);
    }
}
