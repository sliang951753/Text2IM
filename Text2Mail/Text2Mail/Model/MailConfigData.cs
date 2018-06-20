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

namespace Text2Mail.Model
{
    class MailConfigData
    {
        public string ServerName { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Password { get; set; }
    }
}