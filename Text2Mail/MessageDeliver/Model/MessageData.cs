using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MessageDeliver.Model
{
    public class MessageData
    {
        public int Id { get; set; }
        public Guid PublisherId { get; set; }
        public string PublisherDisplayName { get; set; }
        public Guid MessageId { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Forwarded { get; set; }
    }
}
