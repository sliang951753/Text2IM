using MessageDeliver.Domain.Notification;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace MessageDeliver
{
    public class WebsocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWSHandler _handler;

        public WebsocketMiddleware(RequestDelegate next, IWSHandler handler)
        {
            _next = next;
            _handler = handler;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!context.WebSockets.IsWebSocketRequest)
                    return;

                var socket = await context.WebSockets.AcceptWebSocketAsync();
                var publisherId = context.Request.Query[NotificationConnection.PublisherIdKey];
                var connectionExtra = new Dictionary<string, string>();

                connectionExtra.Add(NotificationConnection.PublisherIdKey, publisherId);
                var connection = new NotificationConnection(socket, connectionExtra);

                if (!_handler.OnAccept(connection))
                {
                    connection.Close();
                    return;
                }

                _handler.OnConnected(connection);

                byte[] buffer = new byte[1024];

                while (true)
                {
                    try
                    {
                        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                        {
                            _handler.OnClose(connection);
                        }
                    }
                    catch(WebSocketException e)
                    {
                        _handler.OnClose(connection);

                        break;
                    }
                }
            }
            catch(Exception e)
            {

            }
            finally
            {
            }
        }
    }
}
