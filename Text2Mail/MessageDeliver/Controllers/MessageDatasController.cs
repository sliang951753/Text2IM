using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageDeliver.Model;
using MessageDeliver.Model.DTO;
using MessageDeliver.Domain;
using MessageDeliver.Service.Contract;

namespace MessageDeliver.Controllers
{
    [Produces("application/json")]
    [Route("api/MessageDatas")]
    public class MessageDatasController : Controller
    {
        private readonly IMessageDataService _messageDataService;
        private readonly IMessageNotificationService _messageNotificationService;

        public MessageDatasController(IMessageDataService messageDataService, IMessageNotificationService messageNotificationService)
        {
            _messageDataService = messageDataService;
            _messageNotificationService = messageNotificationService;
        }

        // GET: api/MessageDatas
        [HttpGet]
        public IEnumerable<MessageDataDto> GetMessageDataItems()
        {
            return _messageDataService.GetAll();
        }
        
        [HttpGet("PublisherId/{publisherId:guid}")]
        public IEnumerable<MessageDataDto> GetMessageDataItems([FromRoute] Guid publisherId, [FromQuery] bool forwarded)
        {
            return _messageDataService.GetByPublisherId(publisherId, forwarded);
        }

        [HttpGet("PublisherId/{publisherId:guid}/AfterDate/{afterDate:datetime}")]
        public IEnumerable<MessageDataDto> GetMesssageDataItems([FromRoute] Guid publisherId, [FromRoute] DateTime afterDate)
        {
            return _messageDataService.GetLatestByPublisherId(publisherId, afterDate);
        }

        [HttpGet("MessageId/{messageId:guid}")]
        public MessageDataDto GetMessageDataItems([FromRoute] Guid messageId)
        {
            return _messageDataService.GetByMessageId(messageId);
        }

        // GET: api/MessageDatas/5
        [HttpGet("{id}")]
        public MessageDataDto GetMessageData([FromRoute] int id)
        {
            return _messageDataService.GetById(id);
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

            try
            {
                await Task.Factory.StartNew(() => _messageDataService.Update(messageData));
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

            await Task.Factory.StartNew(() =>{
                _messageDataService.AddOne(messageData);
                _messageNotificationService.NotifySubscribers(messageData);
                }
            );

            return CreatedAtAction("GetMessageData", new { id = messageData.Id }, MessageDataDto.FromEntity(messageData));
        }

        // DELETE: api/MessageDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messageData = _messageDataService.GetById(id);
            if (messageData == null)
            {
                return NotFound();
            }

            await Task.Factory.StartNew(() => _messageDataService.Delete(id));

            return Ok(messageData);
        }

        private bool MessageDataExists(int id)
        {
            return _messageDataService.GetById(id) != null;
        }
    }
}