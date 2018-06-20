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

namespace Text2Mail.Model
{
    class DeviceInformation
    {
        private readonly string _name;
        private readonly Guid _id;

        public string Name { get { return _name; } }
        public Guid Id { get { return _id; } }

        public DeviceInformation(string name, Guid id)
        {
            _name = name;
            _id = id;
        }
    }
}