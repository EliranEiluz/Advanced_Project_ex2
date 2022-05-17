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


        [Route("api/contacts/{id}/messages")]
        [HttpGet]
        public async Task<ActionResult<List<Message>>> GetMessages(string id)
        {
            string currentUser = this.User.Claims.First(i => i.Type == "UserId").Value;
            List<Message> messages = await _service.GetMessagesWithContact(currentUser, id);            
            return messages;      
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



        [Route("api/contacts/{id}/messages/{id2}")]
        [HttpPut]
        public async Task<IActionResult> PutMessage(int id, int id2, MesssageContent mesContent)
        {
            var result = await _service.UpdateMessageById(id2, mesContent.Content);
            if (result == 1)
            {
                return NotFound();
            }

            return StatusCode(204);
        }


        
        [HttpPost]
        [Route("api/contacts/{id}/messages")]
        public async Task<ActionResult> PostMessage(string id, [FromBody] MesssageContent mesContent)
        {
            string currentUser = this.User.Claims.First(i => i.Type == "UserId").Value;
            int result = await _service.postMessage(currentUser, id, mesContent.Content);
            if(result == 1)
            {
                return NoContent();
            }

            return StatusCode(201);
        }


        [Route("api/contacts/{id}/messages/{id2}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(int id, int id2)
        {
            var result = await _service.DeleteMessageById(id2);
            if (result == 1)
            {
                return NotFound();
            }
            return StatusCode(204);
        }


        [Route("api/transfer")]
        [HttpPost]
        public async Task<ActionResult<Message>> transfer(TransferParams mesContent)
        {
            int result = await _service.Transfer(mesContent.From, mesContent.To, mesContent.Content);
            if (result == 1)
            {
                return NoContent();
            }

            return StatusCode(201);
        }

    }
}
