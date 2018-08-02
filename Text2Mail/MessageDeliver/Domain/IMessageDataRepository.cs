using MessageDeliver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Domain
{
    public interface IMessageDataRepository : IRepository<MessageData, int>
    {
    }
}
