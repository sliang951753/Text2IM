using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using Text2Mail.Database.Tables;

namespace Text2Mail.Database
{
    class MessageDB
    {
        class DataSource<T> : IDataSource<T> where T : new()
        {
            private readonly TableQuery<T> _query;

            public DataSource(SQLiteConnection db)
            {
                _query = db.Table<T>();
            }

            public void DeleteAll()
            {
                _query.Delete(x => true);
            }

            public void DeleteOne(T row)
            {
                _query.Delete(x => x.Equals(row));
            }

            public IEnumerable<T> GetAll()
            {
                return _query.AsEnumerable();
            }

            public int GetCount()
            {
                return _query.Count();
            }

            public IEnumerable<T> GetSpecific(Func<T, bool> predicate)
            {
                return _query.Where(predicate).AsEnumerable();
            }
        }

        public const string Name = "t2mdb.db3";

        public static MessageDB Create() { return new MessageDB(); }

        private readonly SQLiteConnection _db;

        private MessageDB()
        {
            string dbPath = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                Name);

            _db = new SQLiteConnection(dbPath);
            
            InitializeTables();
        }

        private void InitializeTables()
        {
            _db.CreateTable<MessageData>(CreateFlags.None);
        }

        public void Insert(IEnumerable<MessageData> messageData)
        {
            _db.InsertAll(messageData, typeof(MessageData));
        }

        public IDataSource<MessageData> GetMessageDataSource()
        {
            return new DataSource<MessageData>(_db);
        }

        public void Close()
        {
            _db.Close();
        }
    }
}