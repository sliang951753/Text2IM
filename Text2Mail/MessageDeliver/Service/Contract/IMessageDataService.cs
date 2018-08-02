using MessageDeliver.Model;
using MessageDeliver.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Service.Contract
{
    public interface IMessageDataService
    {
        void AddOne(MessageData data);
        IEnumerable<MessageDataDto> GetAll();
        MessageDataDto GetById(int id);
        MessageDataDto GetByMessageId(Guid messageId);
        IEnumerable<MessageDataDto> GetByPublisherId(Guid publihserId, bool forwarded);
        IEnumerable<MessageDataDto> GetLatestByPublisherId(Guid publisherId, DateTime afterDate);
        void Update(MessageData messageData);
        void Delete(int id);
    }
}
