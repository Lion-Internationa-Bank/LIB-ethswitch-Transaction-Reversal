
using LIB_Documentmanagement.Application.Interfaces;
using LIB_Documentmanagement.Application.Services;
using LIB_Documentmanagement.DAL.Interface;
using LIB_Documentmanagement.Infra.Data.Repository;
using LIB_TransactionReversal.Application.Interfaces;
using LIB_TransactionReversal.Application.Services;
using LIB_TransactionReversal.DAL.Interface;
using LIB_TransactionReversal.Infra.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LIB_Usermanagement.UI.Infrastructure
{
    internal class RegisterAPIResources : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            //services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            
            services.AddScoped<ITransactionReversalService, TransactionReversalService>();
            services.AddScoped<ITransactionReversalRepository, TransactionReversalRepository>();           
            
            services.AddScoped<IEthswitchTransactionImportService, EthswitchTransactionImportService>();
            services.AddScoped<IEthswitchTransactionImportRepository, EthswitchTransactionImportRepository>();
            services.AddScoped<ILibOutgoingTransactionRepository, LibOutgoingTransactionRepository>();
            services.AddScoped<ICbsTransactionImportRepository, CbsTransactionImportRepository>();
            services.AddScoped<ILibIncomingTransactionRepository, LibIncomingTransactionRepository>();
            services.AddScoped<ITransactionAdjustementRepository, TransactionAdjustementRepository>();
            services.AddScoped<ITransactionAdjustementService, TransactionAdjustementService>();

        }
    }
}
