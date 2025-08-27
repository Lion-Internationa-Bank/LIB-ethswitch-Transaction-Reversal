using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LIB_Usermanagement.DAL;
using LIB_TransactionReversal.DAL.Contexts;
using System;


namespace LIB_Usermanagement.UI.Infrastructure
{
    internal class RegisterDB : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {


            services.AddDbContext<TrasactionReversalDbContext>(options =>
              options.UseMySQL(config["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("LIB_TransactionReversal.DAL")));

            services.AddDbContext<CBSDbContext>(options =>
             options.UseOracle(config["ConnectionStrings:CoreConnection"], b => b.MigrationsAssembly("LIB_Usermanagement.API")));
        }
    }
}
