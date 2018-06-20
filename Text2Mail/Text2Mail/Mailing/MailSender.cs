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
    class MailSender : IMailSender
    {
        public string ServerName { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Password { get; set; }

        public void Send(string subject, string body)
        {
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ServerName, Port);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(FromAddress, Password);

            System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress(FromAddress, "text2Mail");
            System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(ToAddress);
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(from, to);
            mail.Subject = subject;
            mail.Body = body;

            smtp.Send(mail);

        }
    }
}