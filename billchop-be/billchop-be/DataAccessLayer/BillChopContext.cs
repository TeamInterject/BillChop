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
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Bill> Bills => Set<Bill>();

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(ConnectionStringResolver.GetBillChopDbConnectionString());
            }

            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            // Complicated relational mapping goes here
        }
    }
}
