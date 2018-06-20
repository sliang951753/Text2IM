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

namespace Text2Mail.Services.Model
{
    class MessageDataREST
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