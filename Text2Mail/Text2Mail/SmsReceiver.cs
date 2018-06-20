using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Telephony;
using Android.Provider;
using Android.Net;
using System.Net;
using Android.App.Job;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Text2Mail.Model;

namespace Text2Mail
{
    [BroadcastReceiver(Enabled = true, Exported = true, Name = "Text2Mail.SMSReceiver", Label = "SMS Receiver")]
    [IntentFilter(new[] { Telephony.Sms.Intents.SmsReceivedAction })]
    class SmsReceiver : BroadcastReceiver
    {
        private IEnumerable<SmsData> ExtractMessages(SmsMessage[] messages)
        {
            List<SmsData> dataList = new List<SmsData>();

            foreach (SmsMessage message in messages)
            {
                SmsData data = dataList.FirstOrDefault((x) =>
                {
                    if (x.PhoneNumber.Equals(message.DisplayOriginatingAddress))
                        return true;
                    return false;
                });

                if (data == default(SmsData))
                {
                    data = new SmsData(Guid.NewGuid())
                    {
                        PhoneNumber = message.DisplayOriginatingAddress,
                        Body = message.DisplayMessageBody,
                        Timestamp = message.TimestampMillis
                    };

                    dataList.Add(data);
                }
                else
                {
                    data.Body += message.DisplayMessageBody;
                }
            }

            return dataList;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(Telephony.Sms.Intents.SmsReceivedAction))
            {
                SmsMessage[] messages = Telephony.Sms.Intents.GetMessagesFromIntent(intent);
                IEnumerable<SmsData> smsDataList = ExtractMessages(messages);

                Intent mailIntent = new Intent(context, typeof(MailPushService));
                string json = JsonConvert.SerializeObject(smsDataList);

                mailIntent.PutExtra("object", json);
                context.StartService(mailIntent);

                JobInfo.Builder builder = new JobInfo.Builder(TextPersistenceService.GetNewJobId(),
                    new ComponentName(context, Java.Lang.Class.FromType(typeof(TextPersistenceService))));
                PersistableBundle bundle = new PersistableBundle();

                bundle.PutString("object", json);

                builder.SetExtras(bundle);
                builder.SetMinimumLatency(500);
                builder.SetOverrideDeadline(1000);
                
                JobInfo jobInfo = builder.Build();

                JobScheduler jobScheduler = (JobScheduler)context.GetSystemService(Context.JobSchedulerService);
                jobScheduler.Schedule(jobInfo);

            }
        }
    }
}