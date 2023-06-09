using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly ApplicationDbContext _dbcontext;

        public todoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("MyTasks")]
        [Authorize]
        public ActionResult<IEnumerable<todoModel>> GetUserTodos()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid username.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var userTodos = _context.Todos.Where(t => t.UserId == user.UserId).ToList();

            return Ok(userTodos);
        }
        [HttpGet("MyInfo")]
        [Authorize]
        public ActionResult<UserInfoDTO> GetUserinfo()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var x = _context.Users.FirstOrDefault(o => o.Username == username);
            var UserInfo = new UserInfoDTO
            {
                UserId = x.UserId,
                Username = username,
                Phonenumber = x.Phonenumber,
                Address = x.Address
            };
            return Ok(UserInfo);
        }


        [HttpPost("CreateTask")]
        [Authorize]
        public ActionResult<todoModel> CreateTodo([FromBody] todoModel todos)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid username.");
            }

            if (_context.Todos.FirstOrDefault(u => u.Title.ToLower() == todos.Title.ToLower() && u.UserId == todos.UserId) != null)
            {
                ModelState.AddModelError("Custom Error", "Title Already Exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (todos.TaskId > 0)
            {
                return BadRequest(todos);
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            todos.UserId = user.UserId;

            _context.Todos.Add(todos);
            _context.SaveChanges();

            return Ok(todos);
        }




        [HttpDelete("DeleteTask")]
        [Authorize]
        public ActionResult DeleteTodo(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var todo = _context.Todos.FirstOrDefault(x => x.TaskId == id);
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
        [HttpDelete("deleteUserId")]
        [Authorize]
        public ActionResult DeleteUser()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var x = _context.Users.FirstOrDefault(o => o.Username == username);
            if (x != null)
            {
                _context.Users.Remove(x);
                _context.SaveChanges();
            }

            return Ok();
        }


    }
}

