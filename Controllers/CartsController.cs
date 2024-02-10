using FirstWebApiProject_E_Commerce_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstWebApiProject_E_Commerce_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ECommerceV02Context _db;
        public CartsController(ECommerceV02Context db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpGet("GetCarts")]
        public async Task<IActionResult> GetCarts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
            {
                return NotFound();
            }
            var carts =_db.Carts.Where(c=>c.user.Id==userId); 
            return Ok(carts);   
        }
        [HttpPost("AddCart")]
        public async Task<ActionResult> addCarts(int productId,int Quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Cart cart = new Cart
            {
                user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Product = _db.Products.Where(m => m.Id == productId).FirstOrDefault(),
                Quantity = Quantity
            };
            cart.Product.Stock -= Quantity;
            await _db.Carts.AddAsync(cart);
            await _db.SaveChangesAsync();
            return Ok("succeed");
        }
        [HttpPut("UpdateCart/{id}")]
        public async Task<ActionResult> updateCart(int id,int Quantity)
        {
            Cart cart = _db.Carts.Where(c => c.Id == id).FirstOrDefault();
            cart.Quantity += Quantity;
            cart.Product=_db.Products.Where(m=>m.Id==cart.ProductId).FirstOrDefault();
            cart.Product.Stock -= Quantity;
            _db.Carts.Update(cart);
            await _db.SaveChangesAsync();
            return Ok("succeed");
        } 
        [HttpDelete("DeleteCart/{id}")]
        public async Task<ActionResult> deleteCart(int id)
        { 
            Cart cart = _db.Carts.Where(c => c.Id == id).FirstOrDefault();
            _db.Carts.Remove(cart);
            await _db.SaveChangesAsync();
            return Ok(cart);
        }
    }
}
