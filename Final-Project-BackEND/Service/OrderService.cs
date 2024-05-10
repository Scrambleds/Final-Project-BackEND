using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Repositorys;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_BackEND.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository) 
        { 
            _orderRepository = orderRepository;
        }
        public JsonResult GetOrderList()
        {
            var data = _orderRepository.GetOrderList().Result;
            return data;
        }

        public JsonResult GetOrder(int id)
        {
            var data = _orderRepository.GetOrder(id).Result;
            return data;
        }

        public JsonResult SaveOrder(OrderVendor order)
        {
            var data = _orderRepository.SaveOrder(order).Result;
            return data;
        }

        public JsonResult UpdateOrder(OrderVendor order)
        {
            var data = _orderRepository.UpdateOrder(order).Result;
            return data;
        }

        public JsonResult DeleteOrder(int id) 
        {
            var data = _orderRepository.DeleteOrder(id).Result; 
            return data;
        }
    }
}