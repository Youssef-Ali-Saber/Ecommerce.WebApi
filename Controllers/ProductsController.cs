using FirstWebApiProject_E_Commerce_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApiProject_E_Commerce_.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ECommerceV02Context _db;
        public ProductsController(ECommerceV02Context db, UserManager<User> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpGet("Products")]
        public ActionResult getallProducts()
        {
            var d = _db.Products;
            return Ok(d);
        }

        [HttpGet("ProductId/{id}")]
        public ActionResult getproduct(int id)
        {
            var d = _db.Products.Where(m => m.Id  == id).FirstOrDefault();
            if (d == null)
                return NotFound();
            return Ok(d);
        }

        [HttpGet("SKU/{SKU}")]
        public ActionResult getproduct(string SKU)
        {
            var d   = _db.Products.Where(m => m.Sku == SKU);
            return Ok(d);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public ActionResult postproduct(Product product)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _db.Add(product);
            _db.SaveChanges();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public ActionResult putproduct(int id ,Product? product)
        {
            if(product == null)
            {
                var m = _db.Products.Where(v => v.Id == id).FirstOrDefault();
                if (m == null)
                    return BadRequest("Id of Product Is Requred");
                else
                    return BadRequest($"new data is requred to update product that has id:{id}");
            }
            product.Id = id;
            _db.Products.Update(product);
            _db.SaveChanges();
            product=_db.Products.Where(m => m.Id == id).FirstOrDefault();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public ActionResult deleteProduct(int id)
        {
            
            var m = _db.Products.Where(n => n.Id == id).FirstOrDefault();
            if (m == null)
                return NotFound();
            _db.Products.Remove(m);
            _db.SaveChanges();
            return Ok(m);
        }
    }
}
