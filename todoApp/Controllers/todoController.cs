using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
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
        [HttpGet]
        public ActionResult<IEnumerable<todoModel> >GetTodo()
        {
            return Ok(_context.Todos.ToList());
        }

        [HttpPost]
        public ActionResult<todoModel> CreateTodo([FromBody]todoModel todos)
        {
            //if(!modelstate.isvalid)
            //{
            //    return badrequest(modelstate);
            //}
            if (_context.Todos.FirstOrDefault(u => u.Title.ToLower() == todos.Title.ToLower())!=null)
            {
                ModelState.AddModelError("Custom Error", "Title Already Exists");
            }
            if (todos == null)
            {
                return BadRequest(todos);
            }
            if (todos.Id > 0)
            {
                return BadRequest(todos);
            }
            
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
            var todo= _context.Todos.FirstOrDefault(x => x.Id == id);
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
