using BillChopBE.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BillChopBE.DataAccessLayer
{
    public class BillChopContext : DbContext
    {
        private readonly ILoggerFactory? loggerFactory;

        public BillChopContext(ILoggerFactory loggerFactory) : base()
        {
            this.loggerFactory = loggerFactory;
        }

        public BillChopContext(DbContextOptions<BillChopContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<Bill> Bills => Set<Bill>();

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
        {
            if (!options.IsConfigured)
            {
                options
                    .UseLoggerFactory(loggerFactory)
                    .UseLazyLoadingProxies()
                    .UseSqlServer(ConnectionStringResolver.GetBillChopDbConnectionString());
            }

            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelbuilder.Entity<User>()
                .HasMany(u => u.Loans)
                .WithOne(e => e.Loanee)
                .OnDelete(DeleteBehavior.NoAction);

            modelbuilder.Entity<User>()
                .HasMany(u => u.Bills)
                .WithOne(b => b.Loaner)
                .OnDelete(DeleteBehavior.NoAction);

            modelbuilder.Entity<Group>()
                .HasMany(g => g.Users)
                .WithMany(u => u.Groups);

            modelbuilder.Entity<Group>()
                .HasMany(g => g.Bills)
                .WithOne(b => b.GroupContext);

            modelbuilder.Entity<Bill>()
                .HasMany(b => b.Loans)
                .WithOne(e => e.Bill)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
