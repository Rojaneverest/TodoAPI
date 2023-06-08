﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.AccessControl;
using System.Security.Claims;
using todoApp.Data;
using todoApp.Models;

namespace todoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class todoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public todoController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("todos")]
        [Authorize]
        public ActionResult<IEnumerable<todoModel>> GetUserTodos()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid username.");
            }

            var user = _context.DTOUsers.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var userTodos = _context.Todos.Where(t => t.UserId == user.UserId).ToList();

            return Ok(userTodos);
        }



        [HttpPost]
        [Authorize]
        public ActionResult<todoModel> CreateTodo([FromBody] todoModel todos)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid username.");
            }

            if (_context.Todos.FirstOrDefault(u => u.Title.ToLower() == todos.Title.ToLower()) != null)
            {
                ModelState.AddModelError("Custom Error", "Title Already Exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (todos.Id > 0)
            {
                return BadRequest(todos);
            }

            var user = _context.DTOUsers.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            todos.UserId = user.UserId;

            _context.Todos.Add(todos);
            _context.SaveChanges();

            return Ok(todos);
        }




        [HttpDelete("{id:int}")]
        public ActionResult DeleteTodo(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var todo = _context.Todos.FirstOrDefault(x => x.Id == id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                _context.SaveChanges();
            }
            if (todo == null)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
