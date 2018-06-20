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
using SQLite;

namespace Text2Mail.Database.Tables
{
    class TableBase
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        public TableBase()
        {

        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            TableBase rhs = obj as TableBase;

            if (rhs != null)
                return rhs.Id == this.Id;

            return false;
        }
    }
}