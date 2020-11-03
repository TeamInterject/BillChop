using BillChopBE.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BillChopBE.Extensions
{
    public static class WebHostExtensions
    {
        public static IHost MigrateBillChopDb(this IHost webHost)
        {
            var connectionString = ConnectionStringResolver.GetBillChopDbConnectionString();
            var options = new DbContextOptionsBuilder<BillChopContext>()
                    .UseSqlServer(connectionString)
                    .UseLazyLoadingProxies()
                    .Options;

            var dbContext = new BillChopContext(options);
            dbContext.Database.Migrate();

            return webHost;
        }
    }
}
