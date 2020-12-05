using System;
using AutoMapper;
using BillChopBE.Controllers.Models;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Services.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BillChopBE.Extensions
{
    public static class ServiceCollectionAutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = CreateMapperConfig();
            var mapper = config.CreateMapper();

            return services.AddSingleton(mapper);
        }

        public static MapperConfiguration CreateMapperConfig() {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserWithToken, ApiUserWithToken>();
                cfg.CreateMap<UserWithoutPassword, ApiUser>();
                cfg.CreateMap<User, ApiUser>();
                cfg.CreateMap<Group, ApiGroup>();
                cfg.CreateMap<Loan, ApiLoan>();
                cfg.CreateMap<Bill, ApiBill>();
                cfg.CreateMap<Payment, ApiPayment>();          
            });

            return config;
        }
    }
}