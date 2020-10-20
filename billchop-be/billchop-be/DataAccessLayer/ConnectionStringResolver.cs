using Microsoft.Extensions.Configuration;
using System.IO;

namespace BillChopBE.DataAccessLayer
{
    public class ConnectionStringResolver
    {
        public static IConfigurationRoot BuildConfiguration() 
        {
            return new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .AddJsonFile("appsettings.Development.json", true)
              .Build();
        }

        public static string GetBillChopDbConnectionString() => BuildConfiguration().GetConnectionString("BillChopDb");
    }
}