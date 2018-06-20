using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageDeliver.Model;

namespace MessageDeliver.Controllers
{
    [Produces("application/json")]
    [Route("api/MessageDatas")]
    public class MessageDatasController : Controller
    {
        private readonly MessageDataContext _context;

        public MessageDatasController(MessageDataContext context)
        {
            _context = context;
        }

        // GET: api/MessageDatas
        [HttpGet]
        public IEnumerable<MessageData> GetMessageDataItems()
        {
            return _context.MessageDataItems;
        }
        
        [HttpGet("PublisherId/{publisherId}")]
        public IEnumerable<MessageData> GetMessageDataItems([FromRoute] Guid publisherId, [FromQuery] bool forwarded)
        {
            var messageData = _context.MessageDataItems.Where(m => m.PublisherId.Equals(publisherId) && (m.Forwarded == forwarded));

            if (messageData == null)
            {
                return new List<MessageData>();
            }

            return messageData;
        }

        [HttpGet("PublisherId/{publisherId}/{messageId}")]
        public async Task<IActionResult> GetMessageDataItems([FromRoute] Guid publisherId, [FromRoute] Guid messageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messageData = await _context.MessageDataItems
                .SingleOrDefaultAsync(
                    m => m.PublisherId.Equals(publisherId) && m.MessageId.Equals(messageId));

            if (messageData == null)
            {
                return NotFound();
            }

            return Ok(messageData);
        }

        // GET: api/MessageDatas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messageData = await _context.MessageDataItems.SingleOrDefaultAsync(m => m.Id == id);

            if (messageData == null)
            {
                return NotFound();
            }

            return Ok(messageData);
        }

        // PUT: api/MessageDatas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessageData([FromRoute] int id, [FromBody] MessageData messageData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != messageData.Id)
            {
                return BadRequest();
            }

            _context.Entry(messageData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MessageDatas
        [HttpPost]
        public async Task<IActionResult> PostMessageData([FromBody] MessageData messageData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MessageDataItems.Add(messageData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessageData", new { id = messageData.Id }, messageData);
        }

        // DELETE: api/MessageDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messageData = await _context.MessageDataItems.SingleOrDefaultAsync(m => m.Id == id);
            if (messageData == null)
            {
                return NotFound();
            }

            _context.MessageDataItems.Remove(messageData);
            await _context.SaveChangesAsync();

            return Ok(messageData);
        }

        private bool MessageDataExists(int id)
        {
            return _context.MessageDataItems.Any(e => e.Id == id);
        }
    }
}