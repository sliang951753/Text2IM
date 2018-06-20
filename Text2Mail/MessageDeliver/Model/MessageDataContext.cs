using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MessageDeliver.Model
{
    public class MessageDataContext : DbContext
    {
        public MessageDataContext(DbContextOptions<MessageDataContext> options) : base(options)
        {

        }

        public DbSet<MessageData> MessageDataItems { get; set; }
    }
}
