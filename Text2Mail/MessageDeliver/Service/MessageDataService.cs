using MessageDeliver.Domain;
using MessageDeliver.Model;
using MessageDeliver.Model.DTO;
using MessageDeliver.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Service
{
    public class MessageDataService : IMessageDataService
    {
        private readonly IMessageDataRepository _repository;

        public MessageDataService(IMessageDataRepository repository)
        {
            _repository = repository;
        }

        public void AddOne(MessageData data)
        {
            _repository.Add(data);
        }

        public void Delete(int id)
        {
            var messageData = _repository.GetById(id);

            if(messageData != null)
                _repository.Delete(messageData);
        }

        public IEnumerable<MessageDataDto> GetAll()
        {
            return _repository.GetAll().Select(x => MessageDataDto.FromEntity(x));
        }

        public MessageDataDto GetById(int id)
        {
            var messageData = _repository.GetById(id);

            if (messageData == default(MessageData))
                return default(MessageDataDto);

            return MessageDataDto.FromEntity(messageData);
        }

        public MessageDataDto GetByMessageId(Guid messageId)
        {
            var specification = new Specification<MessageData>(x => x.MessageId.Equals(messageId));
            var messageData = _repository.GetBySpecification(specification).SingleOrDefault();

            if (messageData == default(MessageData))
                return default(MessageDataDto);

            return MessageDataDto.FromEntity(messageData);
        }

        public IEnumerable<MessageDataDto> GetByPublisherId(Guid publisherId, bool forwarded)
        {
            var specification = new Specification<MessageData>(x => x.PublisherId.Equals(publisherId) && (x.Forwarded == forwarded));

            return _repository.GetBySpecification(specification).Select(x => MessageDataDto.FromEntity(x));
        }

        public IEnumerable<MessageDataDto> GetLatestByPublisherId(Guid publisherId, DateTime afterDate)
        {
            var specification = new Specification<MessageData>(x => x.PublisherId.Equals(publisherId) && (x.Timestamp > afterDate));

            return _repository.GetBySpecification(specification).Select(x => MessageDataDto.FromEntity(x));
        }

        public void Update(MessageData messageData)
        {
            _repository.Update(messageData);
        }

       
    }
}
