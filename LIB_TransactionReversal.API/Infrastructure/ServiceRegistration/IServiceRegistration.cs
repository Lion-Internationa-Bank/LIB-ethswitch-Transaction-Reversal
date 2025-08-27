using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIB_Usermanagement.UI.Infrastructure
{
    public interface IServiceRegistration
    {
        void RegisterAppServices(IServiceCollection services, IConfiguration configuration);
    }
}
