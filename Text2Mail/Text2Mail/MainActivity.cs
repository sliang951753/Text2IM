using Android.App;
using Android.Widget;
using Android.OS;

using System.Linq;
using Android.Support.V7.Widget;
using Text2Mail.UI;
using Text2Mail.Database;
using Android.Content;
using Newtonsoft.Json;
using Text2Mail.Database.Tables;
using System;
using Android.Views;
using Android.Runtime;
using System.Collections.Generic;
using Text2Mail.Model;

namespace Text2Mail
{
    [Activity(Label = "text2mail", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public const int DATA_INSERTED_MESSAGE = 0x10;

        private MessageDB _db;

        class DataSourceUpdateListener : BroadcastReceiver
        {
            private Handler _handler;

            public DataSourceUpdateListener(Handler handler)
            {
                _handler = handler;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action.Equals(TextPersistenceService.ACTION_DATA_SOURCE_UPDATED))
                {
                    string json = intent.Extras.GetString("messageData");
                    Message message = new Message();
                    message.Data.PutString("messageData", json);
                    message.What = DATA_INSERTED_MESSAGE;

                    _handler.SendMessage(message);
                }
            }
        }

        private TextView _noMessageTextView;
        private SearchView _searchView;
        private RecyclerView _recyclerView;
        private MessageViewerAdapter _messageViewerAdapter;
        private DataSourceUpdateListener _dataSourceUpdateListener;
        private Handler _handler;


        private void OnDataSourceUpdated(Message message)
        {
            switch (message.What)
            {
                case DATA_INSERTED_MESSAGE:
                    {
                        string json = message.Data.GetString("messageData");
                        MessageData[] insertedData = JsonConvert.DeserializeObject<MessageData[]>(json);
                        _messageViewerAdapter.Insert(insertedData);
                    }
                    break;

                default:
                    break;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Layout.ContextMenu, menu);

            var searchItem = menu.FindItem(Resource.Id.action_search);

            _searchView = searchItem.ActionView.JavaCast<SearchView>();
 
            _searchView.QueryTextSubmit += (sender, args) =>
            {
                Toast.MakeText(this, "You searched: " + args.Query, ToastLength.Short).Show();

            };

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add_dummy:
                    OnButtonAddDummyClicked();
                    break;
                case Resource.Id.action_delete_all:
                    OnButtonDeleteAllClicked();
                    break;

                default:
                    break;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _noMessageTextView = FindViewById<TextView>(Resource.Id.emptyText);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            _recyclerView.ChildViewAdded += (sender, args) => { _noMessageTextView.Visibility = ViewStates.Gone; };
            _recyclerView.ChildViewRemoved += (sender, args) => { if(_recyclerView.ChildCount == 0) _noMessageTextView.Visibility = ViewStates.Visible; };

            _db = MessageDB.Create();

            _messageViewerAdapter = new MessageViewerAdapter(_db.GetMessageDataSource());

            _recyclerView.SetAdapter(_messageViewerAdapter);
            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            _handler = new Handler(x => {
                OnDataSourceUpdated(x);
            });

            _dataSourceUpdateListener = new DataSourceUpdateListener(_handler);
        }

        private void OnButtonDeleteAllClicked()
        {
            _messageViewerAdapter.DeleteAll();
        }

        private void OnButtonAddDummyClicked()
        {
            MessageData dummy = new MessageData()
            {
                Sender = "DummySender",
                Body = "Dummy text message",
                Timestamp = DateTime.Now
            };

            List<SmsData> smsDataList = new List<SmsData>();

            var utcBase = new DateTime(1970, 1, 1);
            var ts = new TimeSpan(DateTime.UtcNow.Subtract(utcBase).Ticks).TotalMilliseconds;

            smsDataList.Add(new SmsData(Guid.Empty) { PhoneNumber = "+12345678", Body = "Dummy text message" , Timestamp = (long)ts });



            Intent mailIntent = new Intent(this, typeof(MailPushService));
            string json = JsonConvert.SerializeObject(smsDataList);

            mailIntent.PutExtra("object", json);
            this.StartService(mailIntent);


            //_messageViewerAdapter.InsertOne(dummy);
        }

        protected override void OnDestroy()
        {
            _db.Close();

            base.OnDestroy();
        }

        protected override void OnResume()
        {
            IntentFilter filter = new IntentFilter();
            filter.AddAction(TextPersistenceService.ACTION_DATA_SOURCE_UPDATED);
            RegisterReceiver(_dataSourceUpdateListener, filter);

            base.OnResume();
        }

        protected override void OnPause()
        {
            UnregisterReceiver(_dataSourceUpdateListener);

            base.OnPause();
        }
    }
}

