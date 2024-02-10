using FirstWebApiProject_E_Commerce_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstWebApiProject_E_Commerce_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistsController : ControllerBase
    {
        private readonly ECommerceV02Context _db;
        public WishlistsController(ECommerceV02Context db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetWishlists()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishLists= _db.Wishlists.Where(m=>m.userId==userId);
            return Ok(wishLists);
        }
        [HttpPost("Add/{id}")]
        public IActionResult AddWishlist(int id)
        {
            var user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var w = _db.Wishlists.Where(m => m.ProductId == id && m.userId == user_Id);
            if (w.Any())
                return Ok("this product Is already added before");
            var wishList = new Wishlist
            {
                userId = user_Id,
                ProductId = id
            };
            _db.Add(wishList);
            _db.SaveChanges();
            return Ok(wishList);
        }
        [HttpPost("Delete/{id}")]
        public IActionResult DeleteWishlist(int id)
        {
            var user_Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishLists = _db.Wishlists.Where(m => m.ProductId == id && m.userId == user_Id);
            var w = wishLists.ToList();
            _db.RemoveRange(wishLists);
            _db.SaveChanges();
            return Ok(w);
        }
    }
}
