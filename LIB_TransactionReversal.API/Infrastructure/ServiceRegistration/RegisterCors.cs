using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LIB_Usermanagement.UI.Infrastructure
{
    internal class RegisterCors : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
        
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.WithOrigins("http://10.1.22.25:4083", "http://localhost:4200", "http://10.1.22.25:4200", "http://10.1.22.25:8089", "http://10.1.10.106:8089")
                           .WithHeaders("*")
                           .WithMethods("GET", "POST", "PUT", "DELETE", "*")
                           .AllowCredentials();

                });
                
            });
        }
    }
}
 