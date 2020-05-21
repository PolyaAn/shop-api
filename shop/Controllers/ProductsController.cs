using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopModels;
using ShopModels.CustomerClasses;
using ShopModels.ProductsClasses;

namespace shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/Products/GetProduct?id=5
        [HttpGet("GetProduct")]
        [Authorize]
        public CreatedAtActionResult Get([FromQuery] Guid id)
        {
            Product product = _context.Products.Find(id);

            if (product == null)
            {
                return CreatedAtAction(nameof(Get), new
                {
                    success = false
                });
            }

            return CreatedAtAction(nameof(Get), new
            {
                success = true,
                result = new
                {
                    id = product.Id,
                    name = product.Name,
                    price = product.Price,
                    count = product.Count
                }
            });
        }

        // POST: api/Products
        [HttpPost]
        [Authorize]
        public CreatedAtActionResult Post([FromBody] Product product)
        {
            if (product.Name == null)
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Продукт должен содержать название"
                });
            if (product.Price == 0)
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Продукт должен содержать цену"
                });
            if (product.Count == 0)
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Продукт должен содержать количество"
                });
            Customer customer = _context.Customers.Find(product.CustomerId);
            if (customer == null)
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Пользователя с таким id не существует!"
                });
            _context.Products.Add(product);
            customer.OrderPrice += product.Price * product.Count;
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new
            {
                success = true,
                result = new
                {
                    id = product.Id,
                    name = product.Name,
                    price = product.Price,
                    count = product.Count
                }
            });
        }

        // DELETE: api/Products?id=5
        [HttpDelete]
        [Authorize]
        public CreatedAtActionResult Delete([FromQuery] Guid id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return CreatedAtAction(nameof(Get), new
                {
                    success = false
                });
            }
            Customer customer = _context.Customers.Find(product.CustomerId);
            _context.Products.Remove(product);
            customer.OrderPrice -= product.Price * product.Count;
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new
            {
                success = true
            });
        }
    }
}