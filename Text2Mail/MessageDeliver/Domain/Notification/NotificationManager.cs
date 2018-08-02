using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Domain.Notification
{
    public class NotificationManager : INotificationManager, IWSHandler
    {
        private readonly List<IWSClientConnection> _connections = new List<IWSClientConnection>();

        public IEnumerable<IWSClientConnection> GetSubscriberConnections(Guid publisherId)
        {
            return _connections.Where(x => MatchPublisherId(x, publisherId));
        }

        public bool OnAccept(IWSClientConnection connection)
        {
            return true;
        }

        public void OnClose(IWSClientConnection connection)
        {
            _connections.Remove(connection);
        }

        public void OnConnected(IWSClientConnection connection)
        {
            _connections.Add(connection);
        }

        private bool MatchPublisherId(IWSClientConnection connection, Guid publisherId)
        {
            try
            {
                string value = connection.ConnectionExtra[NotificationConnection.PublisherIdKey];
                return publisherId.Equals(Guid.Parse(value));
            }
            catch (Exception e)
            {

            }

            return false;
        }
    }
}
