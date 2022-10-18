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
using Microsoft.IdentityModel.Tokens;
using MusicLib.API.Models;
using MusicLib.API.Models.Database;

namespace MusicLib.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _config;
        private DatabaseContext db;
        
        public TokenController(IConfiguration config, DatabaseContext context)
        {
            _config = config;
            db = context;
        }
        

        // POST: api/Token
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(LoginData userData)
        {
            if (userData == null || userData.Email == null || userData.Password == null)
            {
                return BadRequest();
            }

            // TODO: Hash Password
            
            var user = db.Users.FirstOrDefault(u => u.Email == userData.Email && u.Password == userData.Password);

            if (user == null)
            {
                return BadRequest("Wrong Credentials!");
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
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: signIn);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
