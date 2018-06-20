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
using Java.Interop;
using Java.Lang;

namespace Text2Mail.UI
{
    class MessageViewHolder : RecyclerView.ViewHolder
    {
        public TextView PhoneNumberView { get; private set; }
        public TextView TimestampView { get; private set; }
        public TextView MessageView { get; private set; }

        public MessageViewHolder(View itemView) : base(itemView)
        {
            PhoneNumberView = ItemView.FindViewById<TextView>(Resource.Id.phoneNumberView);
            TimestampView = ItemView.FindViewById<TextView>(Resource.Id.timestampView);
            MessageView = ItemView.FindViewById<TextView>(Resource.Id.messageView);
        }
    }
}