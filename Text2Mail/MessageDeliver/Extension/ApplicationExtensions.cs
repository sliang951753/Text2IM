using MessageDeliver.Domain.Notification;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageDeliver.Extension
{
    public static class ApplicationExtensions
    {
        public static IApplicationBuilder MapWebsocket(this IApplicationBuilder app, PathString path, IWSHandler handler)
        {
            return app.Map(path, x => x.UseMiddleware<WebsocketMiddleware>(handler));
        }

        public static IServiceCollection AddNotificationManager(this IServiceCollection services)
        {
            services.AddSingleton<NotificationManager>();
            services.AddSingleton<IWSHandler>(x => x.GetService<NotificationManager>());
            services.AddSingleton<INotificationManager>(x => x.GetService<NotificationManager>());

            return services;
        }
    }
}
