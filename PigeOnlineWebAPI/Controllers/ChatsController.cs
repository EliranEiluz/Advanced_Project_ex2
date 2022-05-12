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
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private IPigeOnlineService _service;
        private readonly IConfiguration _config;

        public ChatsController(IConfiguration config, IPigeOnlineService service)
        {
            _service = service;
            _config = config;
        }

        // GET: api/Chats
        [HttpGet]
        public async Task<ActionResult<List<Chat>>> GetChat()
        {
            string username = this.User.Claims.First(i => i.Type == "UserId").Value;

            List<Chat> result =  await  _service.GetChatsByUsername(username);
            if(result == null)
            {
                return NotFound();
            }
            return result;
        }



        // POST: api/Chats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(string username, string server)
        {
            string currentUser = this.User.Claims.First(i => i.Type == "UserId").Value;
            int result = await _service.AddNewContact(currentUser, username, server);
            if(result == 1)
            {
                return NoContent();
            }
            return Ok();
        }

        // DELETE: api/Chats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            //var chat = await _context.Chat.FindAsync(id);
            //if (chat == null)
            {
                return NotFound();
            }

            //_context.Chat.Remove(chat);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChatExists(int id)
        {
            //return _context.Chat.Any(e => e.Id == id);
            return false;
        }
    }
}
