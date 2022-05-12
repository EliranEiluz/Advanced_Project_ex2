#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PigeOnlineWebAPI;
using PigeOnlineWebAPI.Data;

namespace PigeOnlineWebAPI.Controllers
{
    [Authorize]
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


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _service.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async void PostUser(User user)
        {
            var result = await _service.PostUser(user);
            if(result == 1) {
                //return Conflict();
            }
            else if(result == 2) {
                //throw;
            }
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

    }
}
