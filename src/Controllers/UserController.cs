using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Data;
using api.src.Mappers;
using api.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = _context.Users
                            .Include(u => u.Role)
                            .Include(u => u.Products)
                            .ToList();

            var usersDto = users.Select(u => u.ToUserDto()).ToList();
            return Ok(usersDto);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users
                            .Include(u => u.Role)
                            .Include(u => u.Products)
                            .FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = user.ToUserDto();
            return Ok(userDto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if (role == null)
            {
                return BadRequest("Role not found");
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put([FromRoute] int id, [FromBody] User user)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if (role == null)
            {
                return BadRequest("Role not found");
            }

            var userToUpdate = _context.Users.FirstOrDefault(u => u.Id == id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.Rut = user.Rut;
            userToUpdate.Name = user.Name;
            userToUpdate.Email = user.Email;
            userToUpdate.RoleId = user.RoleId;

            _context.SaveChanges();
            return Ok(userToUpdate);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("User Deleted");
        }

        [HttpPost("{userId:int}/buy/{productId:int}")] // Jorge compra arroz, Pepito compra pan...
        public IActionResult BuyProduct([FromRoute] int userId, [FromRoute] int productId)
        {
            var user = _context.Users
                            .Include(u => u.Products)
                            .FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound("User not found");


            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return NotFound("Product not found");

            if(user.Products.Any( p=> p.Id == productId)) return BadRequest("User already own this product");

            user.Products.Add(product);
            _context.SaveChanges();
            return Ok($"Product {product.Name} bought by {user.Name}");
        }

    }
}