using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Text2Mail.Database;
using Text2Mail.Database.Tables;

namespace Text2Mail.UI
{
    class MessageViewerAdapter : RecyclerView.Adapter
    {
        private IDataSource<MessageData> _dataSource;
        private Stack<MessageData> _viewDataCache;

        public MessageViewerAdapter(IDataSource<MessageData> dataSource)
        {
            _dataSource = dataSource;
            _viewDataCache = new Stack<MessageData>(dataSource.GetAll().OrderBy((data) => { return data.Timestamp; }).ToList());
        }

        public override int ItemCount => _viewDataCache.Count;

        public void DeleteAll()
        {
            int itemCount = ItemCount;

            _dataSource.DeleteAll();
            _viewDataCache.Clear();
            NotifyItemRangeRemoved(0, itemCount);
        }

        public void Insert(MessageData[] messageData)
        {
            foreach(MessageData message in messageData)
            {
                _viewDataCache.Push(message);
                NotifyItemInserted(0);
            }
        }

        public void InsertOne(MessageData messageData)
        {
            _viewDataCache.Push(messageData);
            NotifyItemInserted(0);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MessageViewHolder messageViewHolder = holder as MessageViewHolder;
            MessageData messageData = _viewDataCache.ElementAt(position);

            messageViewHolder.PhoneNumberView.Text = messageData.Sender;
            messageViewHolder.TimestampView.Text = string.Format("{0}/{1}/{2} {3}:{4}:{5}", 
                messageData.Timestamp.Year, messageData.Timestamp.Month, messageData.Timestamp.Day, messageData.Timestamp.Hour, messageData.Timestamp.Minute, messageData.Timestamp.Second);
            messageViewHolder.MessageView.Text = messageData.Body;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MessageCardView, parent, false);
            return new MessageViewHolder(itemView);
        }
    }
}