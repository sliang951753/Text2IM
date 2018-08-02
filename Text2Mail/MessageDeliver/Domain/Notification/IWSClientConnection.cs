using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Domain.Notification
{
    public interface IWSClientConnection
    {
        IReadOnlyDictionary<string, string> ConnectionExtra { get; }
        void SendData(byte[] data);
        void SendText(string text);
        void Close();
    }
}
