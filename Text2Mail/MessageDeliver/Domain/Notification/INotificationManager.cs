using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Domain.Notification
{
    public interface INotificationManager
    {
        IEnumerable<IWSClientConnection> GetSubscriberConnections(Guid publisherId);
    }
}
