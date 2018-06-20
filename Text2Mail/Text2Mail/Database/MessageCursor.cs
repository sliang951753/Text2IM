using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using Text2Mail.Database.Tables;

namespace Text2Mail.Database
{
    class MessageCursor : AbstractCursor
    {
        IEnumerable<MessageData> _result;

        public MessageCursor(IEnumerable<MessageData> result)
        {
            _result = result;
        }

        public override int Count => _result.Count();

        public override string[] GetColumnNames()
        {
            return new string[] { "" };
        }

        public override double GetDouble(int column)
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int column)
        {
            throw new NotImplementedException();
        }

        public override int GetInt(int column)
        {
            throw new NotImplementedException();
        }

        public override long GetLong(int column)
        {
            throw new NotImplementedException();
        }

        public override short GetShort(int column)
        {
            throw new NotImplementedException();
        }

        public override string GetString(int column)
        {
            throw new NotImplementedException();
        }

        public override bool IsNull(int column)
        {
            throw new NotImplementedException();
        }
    }
}