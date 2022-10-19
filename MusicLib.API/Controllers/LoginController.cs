using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicLib.API.Models;
using MusicLib.API.Models.Database;
using MusicLib.API.Models.Login;

namespace MusicLib.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        
        private readonly IConfiguration _config;
        private readonly DatabaseContext db;
        
        public LoginController(IConfiguration config, DatabaseContext context)
        {
            _config = config;
            db = context;
        }
        
        // POST: api/Login
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(LoginData userData)
        {
            if (userData == null || userData.Email == null || userData.Password == null)
            {
                return BadRequest();
            }

            // TODO: Hash Password
            
            var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == userData.Email && u.Password == userData.Password);

            if (user == null)
            {
                return BadRequest(new{ status = "Denied", message ="Wrong Credentials!"});
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", user.ID.ToString()),
                new Claim("DisplayName", user.DisplayName),
                new Claim("UserName", user.UserName),
                new Claim("Email", user.Email),
                new Claim("Role",user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: signIn);

            var userInfo = new LoginUserInfo()
            {
                ID = user.ID,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.Name
            };
            
            return Ok(new { status = "ok", message = "Logged in!", accessToken = new JwtSecurityTokenHandler().WriteToken(token) , user = userInfo});
        }
    }
}
