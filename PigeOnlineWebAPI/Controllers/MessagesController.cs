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
    public class MessagesController : ControllerBase
    {
        private IPigeOnlineService _service;
        private readonly IConfiguration _config;

        public MessagesController(IPigeOnlineService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }

        [Route("api/contacts/{id}/messages/{id2}")]
        [HttpGet]
        public async Task<ActionResult<Message>> GetMessage(int id, int id2)
        {
            var message = await _service.GetMessageById(id2);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Change to get id1,id2.
        [Route("api/contacts/{id}/messages/{id2}")]
        [HttpPut]
        public async Task<IActionResult> PutMessage(int id, int id2, Message message)
        {
            var result = await _service.UpdateMessageById(id2, message);
            if (result == 1)
            {
                return BadRequest();
            } else if (result == 2)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/contacts/{id}/messages")]
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            // _service.CreateMessageByUsername();
            //_context.Message.Add(message);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Messages/5
        [Route("api/contacts/{id}/messages/{id2}")]
        //[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int id2)
        {
            // remove from the messages list of the chats ?
            var result = await _service.DeleteMessageById(id2);
            if (result == 1)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
