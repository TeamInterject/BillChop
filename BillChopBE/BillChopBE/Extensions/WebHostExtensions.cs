using BillChopBE.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
