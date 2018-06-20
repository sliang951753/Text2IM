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
using Newtonsoft.Json.Linq;
using Java.Text;
using Java.Util;
using Text2Mail.Mailing;
using Android.Util;
using System.Threading;
using Text2Mail.Model;
using Newtonsoft.Json;
using Text2Mail.Services;
using Text2Mail.Database.Tables;

namespace Text2Mail
{
    [Service(Enabled = true, Exported = true)]
    class MailPushService : IntentService
    {
        // This is any integer value unique to the application.
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const string MAIL_SERVICE_LOG_TAG = "Text2Mail.MailService";
        public const string PUBLISHER_ID = "{0FBE278D-FA7B-4677-A6AB-B8A9A7E58E27}";


        private MessageDeliverService _deliverService;
        private MailSender _sender;
        private Handler _handler;
        private int _intentCount;
        private Notification.Builder _builder;

        public MailPushService() : base("Text2Mail.MailService")
        {

        }

        public override void OnCreate()
        {
            _deliverService = new MessageDeliverService(new DeviceInformation("My Xperia", Guid.Parse(PUBLISHER_ID)));

            _sender = new MailSender()
            {
                ServerName = "smtp-mail.outlook.com",
                Port = 587,
                FromAddress = "goldsun951753@hotmail.com",
                ToAddress = "sl951753@outlook.com",
                Password = "dragon632"
            };

            _builder = new Notification.Builder(this)
                            .SetContentText(_sender.ToAddress)
                            .SetSmallIcon(Resource.Drawable.ic_dialog_email)
                            .SetOngoing(true);

            _handler = new Handler(new Action<Message>(OnIntentHandled));

            base.OnCreate();
        }

        public void OnIntentHandled(Message message)
        {
            Log.Info(MAIL_SERVICE_LOG_TAG, "OnIntentHandled");
            _intentCount--;

            if(_intentCount > 0)
            {
                var notification = _builder
                            .SetContentTitle(string.Format(Resources.GetString(Resource.String.notification_text), _intentCount))
                            .Build();

                NotificationManager.FromContext(this).Notify(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }
        }

        public override void OnDestroy()
        {

            Log.Info(MAIL_SERVICE_LOG_TAG, "OnDestroy");

            base.OnDestroy();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            _intentCount++;

            var notification = _builder
                .SetContentTitle(string.Format(Resources.GetString(Resource.String.notification_text), _intentCount))
                .Build();

            if(_intentCount == 1)
            {
                // Enlist this instance of the service as a foreground service
                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }
            else
            {
                NotificationManager.FromContext(this).Notify(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }

            return base.OnStartCommand(intent, flags, startId);
        }

        protected override async void OnHandleIntent(Intent intent)
        {
            string json = intent.Extras.GetString("object");

            if (!string.IsNullOrEmpty(json))
            {
                List<SmsData> smsDataList = JsonConvert.DeserializeObject<List<SmsData>>(json);

                var messageDatas = smsDataList.Select(x => x.ToMessageData());
         
                await _deliverService.SendMessageData(messageDatas);

                foreach (SmsData smsData in smsDataList)
                {
                    SendMail(_sender, smsData);
                }
            }

            _handler.SendMessage(new Message());
        }

        private void SendMail(MailSender sender, SmsData smsData)
        {
            string phoneNumber = smsData.PhoneNumber;
            string body = smsData.Body;

            long receivedTime = smsData.Timestamp;
            string messageOrignalTime = DateFormat.DateTimeInstance.Format(new Date(receivedTime));
            DateTime now = DateTime.Now;

            string emailBody = string.Format("{0}\r\nSMS Timestamp: {1} \r\nMail Timestamp: {2} {3}",
                body, messageOrignalTime,
                now.ToShortDateString(), now.ToLongTimeString()
                );

            try
            {
                sender.Send(phoneNumber, emailBody);
            }
            catch (Exception ex)
            {
                Log.Error(MAIL_SERVICE_LOG_TAG, ex.Message);
            }
        }
    }
}