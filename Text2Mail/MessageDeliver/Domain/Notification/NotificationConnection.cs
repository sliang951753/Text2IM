using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageDeliver.Domain.Notification
{
    public class NotificationConnection : IWSClientConnection
    {
        private readonly WebSocket _socket;
        private readonly int _sendBufferSize;
        private readonly Dictionary<string, string> _extra;

        public static readonly int DefaultSendBufferSize = 4 * 1024;
        public static readonly string PublisherIdKey = "publisherId";

        public IReadOnlyDictionary<string, string> ConnectionExtra => _extra;

        public NotificationConnection(WebSocket socket, int sendBufferSize, Dictionary<string, string> extra) :
            this(socket, extra)
        {
            _sendBufferSize = sendBufferSize;
        }

        public NotificationConnection(WebSocket socket, Dictionary<string, string> extra) :
            this(socket)
        {
            _socket = socket;
            _extra = new Dictionary<string, string>(extra);
        }

        public NotificationConnection(WebSocket socket)
        {
            _socket = socket;
            _sendBufferSize = DefaultSendBufferSize;
            _extra = new Dictionary<string, string>();
        }

        public void SendData(byte[] data)
        {
            SendDataInternal(data, WebSocketMessageType.Binary);
        }

        private async void SendDataInternal(byte[] data, WebSocketMessageType messageType)
        {
            if (_socket.State != WebSocketState.Open)
                return;

            int packageCount = data.Length / _sendBufferSize + 1;

            try
            {
                if(packageCount == 1)
                {
                    await _socket.SendAsync(new ReadOnlyMemory<byte>(data), messageType, true, CancellationToken.None);
                }
                else
                {
                    MemoryStream stream = new MemoryStream(data);
                    byte[] buffer = new byte[_sendBufferSize];

                    for (int i = 0; i < packageCount - 1; i++)
                    {
                        stream.Read(buffer, 0, buffer.Length);

                        await _socket.SendAsync(new ReadOnlyMemory<byte>(buffer), messageType, false, CancellationToken.None);
                    }

                    int size = stream.Read(buffer, 0, buffer.Length);
                    await _socket.SendAsync(new ReadOnlyMemory<byte>(data, 0, size), messageType, true, CancellationToken.None);
                }
            }
            catch(TaskCanceledException e)
            {
                await _socket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Task Canceled", CancellationToken.None);
            }
        }

        public void SendText(string text)
        {
            SendDataInternal(Encoding.UTF8.GetBytes(text), WebSocketMessageType.Text);
        }

        public void Close()
        {
            _socket.CloseAsync(WebSocketCloseStatus.Empty, "Server Closed", CancellationToken.None);
        }
    }
}
