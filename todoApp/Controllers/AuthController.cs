using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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


namespace todoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
       


        public AuthController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;

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
        public ActionResult<RegisterDTO> Register(RegisterDTO request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Address = request.Address,
                Phonenumber = request.Phonenumber
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(LoginDTO request)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == request.Username);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            var loginuser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            string token = CreateToken(loginuser);

            return Ok(token);
        }



        private string CreateToken(User User)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, User.Username),
               new Claim(ClaimTypes.Sid, User.UserId.ToString())

            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value!));

            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["jwt:Issuer"],
                _configuration["jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        [HttpGet("GetAllUsers")]
        public ActionResult<UserInfoDTO> GetAllUsers()
        {
            //var x = _dbContext.Users.FirstOrDefault(o => o.UserId == id);
            //var UserInfo = new UserInfoDTO
            //{
            //    UserId = id,
            //    Username = x.Username,
            //    Phonenumber = x.Phonenumber,
            //    Address = x.Address
            //};
            //return Ok(UserInfo);


            var users = _dbContext.Users.Select(u => new UserInfoDTO
            {
                UserId = u.UserId,
                Username = u.Username,
                Phonenumber = u.Phonenumber,
                Address = u.Address
            }).ToList();
            
            return Ok(users);
        }


    }

}
