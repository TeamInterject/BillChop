using BillChopBE.DataAccessLayer.Filters.Factories;
using BillChopBE.Extensions;
using BillChopBE.Middleware;
using BillChopBE.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;

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
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }); ;

            // Example of creating injectable config
            // services.ConfigureValidatableSetting<SomeValidatableConfig>(Configuration.GetSection("SomeSection"));

            services.AddBillChopContext(Configuration.GetConnectionString("BillChopDb"));
            services.AddBillChopRepositories();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<ILoanService, LoanService>();

            services.AddScoped<ILoanDbFilterFactory, LoanDbFilterFactory>();
            services.AddScoped<IBillDbFilterFactory, BillDbFilterFactory>();

            services.AddSwaggerGen(c => 
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            }
            else
            {
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Billchop API v0.0.1-dev.1");
            });

            app.UseHttpsRedirection();

            app.UseMiddleware(typeof(TransactionMiddleware));

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(corsBuilder => corsBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .Build());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
