using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Domain.Notification
{
    public interface IWSHandler
    {
        bool OnAccept(IWSClientConnection connection);
        void OnConnected(IWSClientConnection connection);
        void OnClose(IWSClientConnection connection);
    }
}
