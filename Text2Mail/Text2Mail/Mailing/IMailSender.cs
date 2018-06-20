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

namespace Text2Mail.Mailing
{
    interface IMailSender
    {
        void Send(string subject, string body);
    }
}