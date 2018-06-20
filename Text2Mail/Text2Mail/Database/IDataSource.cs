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
using Text2Mail.Database.Tables;

namespace Text2Mail.Database
{
    interface IDataSource<T> where T : new()
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetSpecific(Func<T, bool> predicate);
        int GetCount();
        void DeleteAll();
        void DeleteOne(T row);
    }
} 