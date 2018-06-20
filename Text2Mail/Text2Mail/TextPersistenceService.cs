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
using Android.App.Job;
using Newtonsoft.Json.Linq;
using Java.Util;
using Java.Text;
using Text2Mail.Mailing;
using Newtonsoft.Json;
using Text2Mail.Model;
using Text2Mail.Database;
using Text2Mail.Database.Tables;

namespace Text2Mail
{
    [Service(Exported = true, Permission = "android.permission.BIND_JOB_SERVICE")]
    class TextPersistenceService : JobService
    {
        public const string ACTION_DATA_SOURCE_UPDATED = "Text2Mail.ActionDataSourceUpdated";

        private PersistenceTask _internalTask;
        private static int JobId = 100;

        public static int GetNewJobId() { return JobId++; }

        private class PersistenceTask : AsyncTask<SmsData, MessageData, bool>
        {
            private JobService _caller;
            private JobParameters _paramters;
            private MessageDB _db;

            public PersistenceTask(JobService caller, JobParameters parameters, MessageDB db)
            {
                _caller = caller;
                _paramters = parameters;
                _db = db;
            }

            protected override void OnPostExecute(bool result)
            {
                if(result)
                {
                    _caller.JobFinished(_paramters, false);
                }

                base.OnPostExecute(result);
            }

            protected override void OnProgressUpdate(params MessageData[] values)
            {
                Intent intent = new Intent(ACTION_DATA_SOURCE_UPDATED);

                intent.PutExtra("messageData", JsonConvert.SerializeObject(values));
                _caller.SendBroadcast(intent);

                base.OnProgressUpdate(values);
            }

            protected override void OnCancelled()
            {
                base.OnCancelled();
            }

            protected override bool RunInBackground(params SmsData[] @params)
            {
                var messageDataList = @params.AsEnumerable().Select((x) => {
                    var ts = TimeSpan.FromMilliseconds(x.Timestamp);
                    var timestamp = new DateTime(1970, 1, 1);
                    var messageTime = timestamp.Add(ts).ToLocalTime();
                   
                    return new MessageData()
                    {
                        Sender = x.PhoneNumber,
                        Body = x.Body,
                        Timestamp = messageTime
                    };
                });

                PublishProgress(messageDataList.ToArray());
                _db.Insert(messageDataList);

                return true;
            }
        }

        public override bool OnStartJob(JobParameters @params)
        {
            string json = @params.Extras.GetString("object");

            if (!string.IsNullOrEmpty(json))
            {
                List<SmsData> smsDataList = JsonConvert.DeserializeObject<List<SmsData>>(json);

                _internalTask = new PersistenceTask(this, @params, MessageDB.Create());
                _internalTask.Execute(smsDataList.ToArray());

                return true;
            }

            return false;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            _internalTask.Cancel(true);

            return true;
        }
    }
}