using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Text2Mail.Database.Tables;
using Text2Mail.Model;
using Text2Mail.Services.Model;

namespace Text2Mail.Services
{
    class MessageDeliverService : RESTfulService<MessageDataREST>
    {
        public const string MessageDeliverServiceUrl = "http://messagedeliver.azurewebsites.net";

        private DeviceInformation _deviceInfo;

        public MessageDeliverService(DeviceInformation deviceId) : base(MessageDeliverServiceUrl)
        {
            _deviceInfo = deviceId;
        }

        public async Task<List<MessageData>> GetForwardedMessageDatas()
        {
            string resourceRoute = string.Format("api/messagedatas/publisherId/{0}", _deviceInfo.Id);

            var result = await GetSpecific(resourceRoute, "forwarded=true");

            return result.Select((x) => new MessageData {
                MessageId = x.MessageId,
                Sender = x.Sender,
                Body = x.Body,
                Timestamp = x.Timestamp,
                Forwarded = x.Forwarded
            }).ToList();
        }

        public async Task<bool> SendMessageData(MessageData messageData)
        {
            return await Create("api/messagedatas", ConvertToRESTful(messageData));
        }

        public async Task<bool> SendMessageData(IEnumerable<MessageData> messageData)
        {
            if (messageData.Count() == 1)
                return await SendMessageData(messageData.FirstOrDefault());

            var restfulData = messageData.Select(x => ConvertToRESTful(x));

            return await Create("api/messagedatas", restfulData);
        }

        private MessageDataREST ConvertToRESTful(MessageData messageData)
        {
            return new MessageDataREST()
            {
                PublisherId = _deviceInfo.Id,
                PublisherDisplayName = _deviceInfo.Name,
                Sender = messageData.Sender,
                Body = messageData.Body,
                Timestamp = messageData.Timestamp,
                Forwarded = messageData.Forwarded,
                MessageId = messageData.MessageId
            };
        }
    }
}