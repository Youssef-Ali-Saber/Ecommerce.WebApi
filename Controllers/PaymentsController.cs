using FirstWebApiProject_E_Commerce_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FirstWebApiProject_E_Commerce_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ECommerceV02Context _db;
        public PaymentsController(ECommerceV02Context db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetDeteils()
        {
            var user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var payment = _db.Payments.Include(m => m.Order).Where(o =>o.Order.userId == user_Id);
            return Ok(payment);
        }
        [HttpGet("Deteils/{id}")]
        public IActionResult GetDeteils(int id)
        {
            var user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var payment = _db.Payments.Include(m=>m.Order).FirstOrDefault(o => o.Id == id && o.Order.userId==user_Id);
            return Ok(payment);
        }
        [HttpPost("Create")]
        public IActionResult CreatePayment(Payment payment)
        {
            payment.Date = DateTime.Now;
            var order= _db.Orders.FirstOrDefault(o => o.Id == payment.OrderId);
            order.Status = "Payed";
            payment.Amount = order.TotalPrice;
            _db.Add(payment);
            _db.SaveChanges();
            return Ok(payment);
        }
    }
}
