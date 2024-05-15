using Microsoft.EntityFrameworkCore;
using ServiceA.Outboxes;

namespace ServiceA.Context
{
    public class ServiceADbContext : DbContext
    {

        public ServiceADbContext(DbContextOptions options) : base(options)
        {

        }
       public DbSet<Outbox> Outbox { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-V0R454\\MSSQL2022DEV;Database=ServiceADb;User ID=sa;Password=mssql2022dev;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
);
        }

    }
}
