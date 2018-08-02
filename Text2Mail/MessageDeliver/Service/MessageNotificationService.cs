using MessageDeliver.Domain.Notification;
using MessageDeliver.Model;
using MessageDeliver.Model.DTO;
using MessageDeliver.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessageDeliver.Service
{
    public class MessageNotificationService : IMessageNotificationService
    {
        private readonly INotificationManager _notificationManager;

        public MessageNotificationService(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public void NotifySubscribers(MessageData messageData)
        {
            var subscribers = _notificationManager.GetSubscriberConnections(messageData.PublisherId);
            var dto = MessageDataDto.FromEntity(messageData);
            var settings = new JsonSerializerSettings();
            subscribers.AsParallel().ForAll(x => {
                string json = JsonConvert.SerializeObject(dto);

                x.SendText(json);
            });
        }
    }
}
