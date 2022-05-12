#nullable disable
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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PigeOnlineWebAPI;
using PigeOnlineWebAPI.Data;

namespace PigeOnlineWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IPigeOnlineService _service;
        private readonly IConfiguration _config;

        public UsersController(IPigeOnlineService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }


        // POST: api/Users/login
        [Route("Login")]
        //[ActionName("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserValidation userValidation)
        {
            var user = await _service.GetUser(userValidation.Username);

            if (user == null || user.Password != userValidation.Password)
            {
                return NotFound();
            }
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, _config["JWTParams:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", user.Username)
            };
            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTParams:SecretKey"]));
            var mac = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["JWTParams:Issuer"],
                _config["JWTParams:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: mac);
            user.Username = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(user);
        }


        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            var result = await _service.PostUser(user);
            if(result == 1 || result == 2) {
                return Conflict("false");
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _config["JWTParams:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", user.Username)
            };
            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTParams:SecretKey"]));
            var mac = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["JWTParams:Issuer"],
                _config["JWTParams:Audience"],
                claims,
                expires:DateTime.UtcNow.AddMinutes(20),
                signingCredentials:mac);
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));


        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _service.DeleteUser(id);
            if (result == 1)
            {
                return NotFound();
            }

            return NoContent();
        }

        //[HttpGet]
        //public async Task<IActionResult> Logout(User user)
        //{
            // return token with expires 0 
        //}

    }
}
