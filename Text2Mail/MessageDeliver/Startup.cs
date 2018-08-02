using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using MessageDeliver.Model;
using MessageDeliver.Domain;
using System.Net.WebSockets;
using MessageDeliver.Extension;
using MessageDeliver.Domain.Notification;
using MessageDeliver.Service.Contract;
using MessageDeliver.Service;

namespace MessageDeliver
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<MessageDataContext>(opt =>
            //   opt.UseInMemoryDatabase("MessageDataList"));
            services.AddDbContext<MessageDataContext>(opt
                 => {
                     //opt.UseSqlServer(Configuration.GetConnectionString("SqlServerLocal"));
                     opt.UseMySql(Configuration.GetConnectionString("MySqlAzure"));
                 });

            services.AddMvc();
            services.AddNotificationManager();
            services.AddTransient<IMessageDataRepository, MessageDataRepository>();
            services.AddTransient<IMessageDataService, MessageDataService>();
            services.AddTransient<IMessageNotificationService, MessageNotificationService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc();
            app.UseWebSockets(new WebSocketOptions() {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            });
            app.MapWebsocket("/notification", app.ApplicationServices.GetService<IWSHandler>());
            
        }
    }
}
