using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MessageDeliver.Model
{
    public class MessageDataContext : DbContext
    {
        public MessageDataContext(DbContextOptions<MessageDataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<MessageData> MessageDataItems { get; set; }
    }
}
