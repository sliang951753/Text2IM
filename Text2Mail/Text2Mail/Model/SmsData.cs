using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Text2Mail.Database.Tables;

namespace Text2Mail.Model
{
    class SmsData
    {
        private readonly Guid _messageId;

        public Guid MessageId { get { return _messageId; } }
        public string PhoneNumber { get; set; }
        public string Body { get; set; }
        public long Timestamp { get; set; }

        public SmsData(Guid messageId)
        {
            _messageId = Guid.NewGuid();
        }

        public MessageData ToMessageData()
        {
            var ts = TimeSpan.FromMilliseconds(Timestamp);
            var timestamp = new DateTime(1970, 1, 1);
            var messageTime = timestamp.Add(ts).ToLocalTime();

            return new MessageData()
            {
                MessageId = MessageId,
                Timestamp = messageTime,
                Sender = PhoneNumber,
                Body = Body,
                Forwarded = false,
                Id = 0
            };
        }
    }
}