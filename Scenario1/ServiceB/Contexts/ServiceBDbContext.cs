using Microsoft.EntityFrameworkCore;
using ServiceB.Inboxes;

namespace ServiceB.Contexts
{
    public class ServiceBDbContext : DbContext
    {

        public ServiceBDbContext(DbContextOptions<ServiceBDbContext> options) : base(options)
        {

        }

        public DbSet<Inbox> Inbox { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-V0R454\\MSSQL2022DEV;Database=ServiceBDb;User ID=sa;Password=mssql2022dev;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        //}

    }
}
