using BillChopBE.DataAccessLayer;
using BillChopBE.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BillChopBE
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Example of creating injectable config obj
            // services.ConfigureValidatableSetting<SomeValidatableConfig>(Configuration.GetSection("SomeSection"));

            services.AddScoped<DbConnection>((serviceProvider) =>
            {
                var dbConnection = new SqlConnection(Configuration.GetConnectionString("BillChopDb"));
                dbConnection.Open();
                return dbConnection;
            });

            services.AddScoped((serviceProvider) =>
            {
                var dbConnection = serviceProvider
                    .GetService<DbConnection>();

                return dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            });

            services.AddScoped((serviceProvider) =>
            {
                var dbConnection = serviceProvider.GetService<DbConnection>();
                return new DbContextOptionsBuilder<BillChopContext>()
                    .UseLazyLoadingProxies()
                    .UseSqlServer(dbConnection)
                    .Options;
            });

            services.AddScoped((serviceProvider) =>
            {
                var transaction = serviceProvider.GetService<DbTransaction>();
                var options = serviceProvider.GetService<DbContextOptions<BillChopContext>>();
                var context = new BillChopContext(options);
                context.Database.UseTransaction(transaction);
                return context;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            }

            app.UseHttpsRedirection();

            app.UseMiddleware(typeof(TransactionMiddleware));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
