using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopModels;
using ShopModels.CustomerClasses;

namespace shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ShopContext _context;

        public CustomersController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        [Authorize(Roles = "admin")]
        public CreatedAtActionResult Get()
        {
            return CreatedAtAction(nameof(Get), new
            {
                success = true,
                result = _context.Customers
                    .Select(c => new
                    {
                        id = c.Id,
                        orderPrice = c.OrderPrice,
                        login = c.Login
                    })
            });
        }

        // GET: api/Customers/GetCustomer?login=nastya
        [HttpGet("GetCustomer")]
        public CreatedAtActionResult Get([FromQuery] string login)
        {
            if (!_context.Customers.Any(c => c.Login == login))
            {
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Пользователя с таким логином не существует"
                });
            }

            Customer customer = _context.Customers.First(c => c.Login == login);

            return CreatedAtAction(nameof(Get), new
            {
                success = true,
                result = new
                {
                    customer = new
                    {
                        id = customer.Id,
                        login = customer.Login,
                        orderPrice = customer.OrderPrice
                    },
                    token = Auth.GenerateToken(true)
                }
            });
        }

        // GET: api/Customer/order?customerId=5
        [HttpGet("order")]
        [Authorize]
        public CreatedAtActionResult GetProducts([FromQuery] Guid customerId)
        {
            Customer customer = _context.Customers.Find(customerId);
            if (customer == null)
            {
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Пользователя с таким id не существует!"
                });
            }

            return CreatedAtAction(nameof(Get), new
            {
                success = true,
                result = _context.Customers
                    .Where(c => c.Id == customerId)
                    .Select(c => c.Products.Select(p => new
                    {
                        id = p.Id,
                        name = p.Name,
                        price = p.Price,
                        count = p.Count
                    })).SingleOrDefault()
            });
        }

        // POST: api/Customers
        [HttpPost]
        public CreatedAtActionResult Post([FromBody] Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Login))
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Логин должен быть заполнен"
                });

            if (_context.Customers.Count<Customer>() != 0)
            {
                bool findCustomer = _context.Customers.Any(c => c.Login == customer.Login);
                if (findCustomer)
                    return CreatedAtAction(nameof(Get), new
                    {
                        success = false,
                        reason = "Пользователь с таким логином уже существует"
                    });
            }

            _context.Customers.Add(customer);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new
            {
                success = true,
                result = new
                {
                    customer = new
                    {
                        id = customer.Id,
                        login = customer.Login,
                        orderPrice = customer.OrderPrice
                    },
                    token = Auth.GenerateToken(true)
                }
            });
        }

        // DELETE: api/Customer?id=5
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public CreatedAtActionResult Delete([FromQuery] Guid id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Пользователя с таким id не существует!"
                });
            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new
            {
                success = true
            });
        }

        // DELETE: api/Customers/ClearOrder?customerId=5
        [HttpDelete("ClearOrder")]
        [Authorize]
        public CreatedAtActionResult ClearOrder([FromQuery] Guid customerId)
        {
            Customer customer = _context.Customers.Find(customerId);
            if (customer == null)
            {
                return CreatedAtAction(nameof(Get), new
                {
                    success = false,
                    reason = "Пользователя с таким id не существует!"
                });
            }

            foreach (var product in _context.Products)
            {
                if (product.CustomerId == customerId) _context.Products.Remove(product);
            }
            customer.OrderPrice = 0;
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new
            {
                success = true
            });
        }
    }
}