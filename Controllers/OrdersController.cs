using FirstWebApiProject_E_Commerce_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FirstWebApiProject_E_Commerce_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceV02Context _db;
        public OrdersController(ECommerceV02Context db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetOrders()
        {
            var user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _db.Orders.Where(m => m.userId == user_Id);
            return Ok(orders);
        }
        [HttpPost("Add")]
        public IActionResult AddOrders(List<int> cartsId)
        {
            var user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = new Order
            {
                userId = user_Id,
                Date = DateTime.UtcNow,
                Status="In Order"
            };
            var orderItems = new List<OrderItem>();
            foreach (var id in cartsId)
            {
                var cart = _db.Carts.Include(q => q.Product).Where(m => m.Id == id).FirstOrDefault();
                orderItems.Add
                (
                    new OrderItem() { OrderId = order.Id, ProductId = cart.Product.Id, Quantity = cart.Quantity, Price = cart.Product.Price }
                );
            }
            order.OrderItems = orderItems;
            order.TotalPrice = order.OrderItems.Sum(m => m.Price*m.Quantity);
            _db.Orders.Add(order);
            _db.SaveChanges();
            return Ok(order);
        }
        [HttpGet("Deteils/{id}")]
        public IActionResult GetOrder(int id)
        {
            var user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _db.Orders.Include(n=>n.OrderItems).Where(m => m.userId == user_Id&&m.Id==id);
            return Ok(order);
        }
        [HttpPut("Cancel/{id}")]
        public IActionResult CancelOrder(int id)
        {
            var user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _db.Orders.Include(n => n.OrderItems).Where(m => m.userId == user_Id && m.Id == id).FirstOrDefault();
            order.Status = "Cancel";
            _db.Orders.Update(order);
            _db.SaveChanges();
            return Ok(order);
        }
    }
}
