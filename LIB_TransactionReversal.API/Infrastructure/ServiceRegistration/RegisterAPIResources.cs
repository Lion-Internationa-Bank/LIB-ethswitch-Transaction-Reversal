
using DataAccess.Repository;
using LIB_Documentmanagement.Application.Interfaces;
using LIB_Documentmanagement.Application.Services;
using LIB_Documentmanagement.DAL.Interface;
using LIB_Documentmanagement.Infra.Data.Repository;
using LIB_TransactionReversal.API.Endpoints.Repository;
using LIB_TransactionReversal.Application.Interfaces;
using LIB_TransactionReversal.Application.Services;
using LIB_TransactionReversal.DAL.Interface;
using LIB_TransactionReversal.Infra.Data.Repository;
using LIB_Usermanagement.Application.Interface;
using LIB_Usermanagement.Application.Services;
using LIB_Usermanagement.DAL.Repository;
using LIB_Usermanagement.Infra.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;


namespace LIB_Usermanagement.UI.Infrastructure
{
    internal class RegisterAPIResources : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            //services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

            
            services.AddScoped<ITransactionReversalService, TransactionReversalService>();
            services.AddScoped<ITransactionReversalRepository, TransactionReversalRepository>();           
            
            services.AddScoped<IEthswitchTransactionImportService, EthswitchTransactionImportService>();
            services.AddScoped<IEthswitchTransactionImportRepository, EthswitchTransactionImportRepository>();
            services.AddScoped<ILibOutgoingTransactionRepository, LibOutgoingTransactionRepository>();
            services.AddScoped<ICbsTransactionImportRepository, CbsTransactionImportRepository>();
            services.AddScoped<ILibIncomingTransactionRepository, LibIncomingTransactionRepository>();
            services.AddScoped<ITransactionAdjustementRepository, TransactionAdjustementRepository>();
            services.AddScoped<ITransactionAdjustementService, TransactionAdjustementService>();
            services.AddScoped<AwachRepository, AwachRepository>();
            services.AddScoped<TelebirrRepository, TelebirrRepository>();
            services.AddScoped<EthswichRepository, EthswichRepository>();
            services.AddScoped<MpesaRepository, MpesaRepository>();
            services.AddScoped<TeleBirrMerchant, TeleBirrMerchant>();


        }
    }
}
