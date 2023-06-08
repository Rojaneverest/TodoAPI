using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using todoApp.Data;
using todoApp.Models;
using todoApp.Services;

namespace todoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _UserService;


        public AuthController(ApplicationDbContext dbContext, IUserService UserService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _UserService = UserService;
            _configuration = configuration;

        }

        //[HttpGet, Authorize]
        //public ActionResult<string> GetMyName()
        //{
        //    return Ok(_UserService.GetMyName());

        //    //var UserName = User?.Identity?.Name;
        //    //var roleClaims = User?.FindAll(ClaimTypes.Role);
        //    //var roles = roleClaims?.Select(c => c.Value).ToList();
        //    //var roles2 = User?.Claims
        //    //    .Where(c => c.Type == ClaimTypes.Role)
        //    //    .Select(c => c.Value)
        //    //    .ToList();
        //    //return Ok(new { UserName, roles, roles2 });
        //}

        [HttpPost("register")]
        public ActionResult<UserDTO> Register(UserDTO request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var userDto = new UserDTO
            {
                Username = user.Username,
                Password = user.PasswordHash
            };

            return Ok(userDto);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDTO request)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            var userDto = new UserDTO
            {
                Username = user.Username,
                Password = user.PasswordHash
            };

            string token = CreateToken(userDto);

            return Ok(token);
        }



        private string CreateToken(UserDTO User)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, User.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}