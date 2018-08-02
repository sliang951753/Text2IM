using MessageDeliver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Service.Contract
{
    public interface IMessageNotificationService
    {
        void NotifySubscribers(MessageData messageData);
    }
}
