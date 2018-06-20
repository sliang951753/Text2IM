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
using SQLite;

namespace Text2Mail.Database.Tables
{
    [Table("Messages")]
    class MessageData : TableBase
    {
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("messageId")]
        public Guid MessageId { get; set; }

        [MaxLength(50), Column("sender")]
        public string Sender { get; set; }

        [Column("body")]
        public string Body { get; set; }

        [Column("forwarded")]
        public bool Forwarded { get; set; }

        public override string ToString()
        {
            return "[" + Sender + "] " + Body;
        }
    }
}