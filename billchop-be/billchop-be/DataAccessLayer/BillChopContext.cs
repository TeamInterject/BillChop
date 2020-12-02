using BillChopBE.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BillChopBE.DataAccessLayer
{
    public class BillChopContext : DbContext
    {
        public BillChopContext() : base()
        {
        }

        public BillChopContext(DbContextOptions<BillChopContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<Bill> Bills => Set<Bill>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(ConnectionStringResolver.GetBillChopDbConnectionString());
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Loans)
                .WithOne(e => e.Loanee)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bills)
                .WithOne(b => b.Loaner)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Users)
                .WithMany(u => u.Groups);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Bills)
                .WithOne(b => b.GroupContext);

            modelBuilder.Entity<Bill>()
                .HasMany(b => b.Loans)
                .WithOne(e => e.Bill)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
