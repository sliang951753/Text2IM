using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageDeliver.Model;


namespace MessageDeliver.Domain
{
    public class MessageDataRepository : IMessageDataRepository
    {
        private readonly MessageDataContext _context;

        public MessageDataRepository(MessageDataContext context)
        {
            _context = context;
        }

        protected MessageDataRepository()
        {

        }

        public MessageData Add(MessageData entity)
        {
            _context.MessageDataItems.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public void Delete(MessageData entity)
        {
            _context.MessageDataItems.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<MessageData> GetAll()
        {
            return _context.MessageDataItems;
        }

        public MessageData GetById(int id)
        {
            return _context.MessageDataItems.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<MessageData> GetBySpecification(Specification<MessageData> spec)
        {
            return _context.MessageDataItems.Where(x => spec.IsSatisfiedBy(x));
        }

        public void Update(MessageData entity)
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
