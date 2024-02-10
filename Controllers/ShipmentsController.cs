using FirstWebApiProject_E_Commerce_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApiProject_E_Commerce_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ShipmentsController : ControllerBase
    {
        private readonly ECommerceV02Context _db;
        public ShipmentsController(ECommerceV02Context db)
        {
            _db = db;
        }
        [HttpPost("Add")]
        public IActionResult AddShipment(Shipment shipment)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id== shipment.OrderId);
            order.Status = "Shipped";
            shipment.Date= DateTime.Now;
            _db.Add(shipment);
            _db.Update(order);
            _db.SaveChanges();
            return Ok(shipment);
        }
        [HttpGet]
        public IActionResult GetShipments()
        {
            var shipments = _db.Shipments;
            return Ok(shipments);
        }
    }
}
