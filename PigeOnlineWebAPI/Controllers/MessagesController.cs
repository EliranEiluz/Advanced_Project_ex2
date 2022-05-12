#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PigeOnlineWebAPI;
using PigeOnlineWebAPI.Data;

namespace PigeOnlineWebAPI.Controllers
{
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


        [Route("api/contacts/:id/messages/:id2")]
        [HttpGet]
        public async Task<ActionResult<Message>> GetMessage(int id, int id2)
        {
            var message = await _service.GetMessageById(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Change to get id1,id2.
        [Route("api/contacts/:id/messages/:id2")]
        [HttpPut]
        public async Task<IActionResult> PutMessage(int id, int id2, Message message)
        {
            var result = _service.UpdateMessageById(id2, message);
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
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
