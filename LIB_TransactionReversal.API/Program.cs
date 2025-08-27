using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
 
namespace LIB_Usermanagement.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //Seed database
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                //try
                //{
                //    var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
                //    databaseInitializer.SeedAsync().Wait();
                //}
                //catch (Exception ex)
                //{
                
                //    throw new Exception( ex.Message);
                //}
            }
            //host.MigrateDatabase();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
