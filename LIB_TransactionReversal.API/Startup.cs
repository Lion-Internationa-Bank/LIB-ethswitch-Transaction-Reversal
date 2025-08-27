using System;
using LIB_Usermanagement.UI.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoWrapper;
using LIB_Usermanagement.DAL.Entity.Account;
using LIB_Usermanagement.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using LIB_TransactionReversal.Infra.Data.Repository;
using LIB_TransactionReversal.DAL.Interface;

namespace LIB_Usermanagement.UI
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
            services.AddAuthorization();


          

            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 7;
                o.User.RequireUniqueEmail = true;

                // Lockout settings
                o.Lockout.AllowedForNewUsers = true; // Allow lockout for new users
                o.Lockout.MaxFailedAccessAttempts = 5; // Max failed attempts
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(3650); // Lockout duratio
            })
         .AddEntityFrameworkStores<TrasactionReversalDbContext>()
         .AddDefaultTokenProviders();

            var jwtSettings = Configuration.GetSection("JwtSettings");
            //var secretKey = Environment.GetEnvironmentVariable("SECRET");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new
                    SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecurityKey"]))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MPESA", policy =>
                    policy.RequireClaim("MPESA", "true"));
            });
            services.AddServicesInAssembly(Configuration);
            //services.AddApiVersioning();
            //services.AddMvcCore()
            //      .AddNewtonsoftJson(options =>
            //      {
            //          options.UseMemberCasing();
            //          options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //      });

            services.AddAutoMapper(typeof(Startup));
            var data = Configuration.GetConnectionString("DefaultConnection1");
            services.AddHangfire(configuration => configuration
            .UsePostgreSqlStorage(Configuration.GetConnectionString("DefaultConnection1")));

            // Add the Hangfire server
            services.AddHangfireServer();


            services.AddMvc(config =>
            {

            }).AddXmlSerializerFormatters();
            services.AddControllers().AddXmlSerializerFormatters();
            //services.AddControllers().AddJsonOptions
            //    (options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHttpContextAccessor();
            //AutoWrapper Exception Settings
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 10485760; // 10 MB
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 10485760; // 10 MB
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config,
            IServiceProvider _serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }




            //Enable Swagger and SwaggerUI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LIB Transaction Web Services V 1.0");
            });
            //AutoWrapper
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            {
                IgnoreWrapForOkRequests = true,
                IsDebug = true,
                EnableResponseLogging = false,
                EnableExceptionLogging = false,
                UseApiProblemDetailsException = true,
                IsApiOnly = false
            });
            app.UseHangfireDashboard();
           

            //// Ensure that Hangfire server is running
            app.UseHangfireServer();

            app.UseHttpsRedirection();
            app.UseRouting();
            //Enable CORS
            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
               
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            RecurringJob.AddOrUpdate<ILibOutgoingTransactionRepository>(job => job.GetBatchEthswitchOutgoingTransaction(), "0 7 * * *"); // Run on the mornong at 7 AM
            RecurringJob.AddOrUpdate<LibIncomingTransactionRepository>(job => job.GetBatchEthswitchIncommingTransaction(), "0 7 * * *"); // Run on the mornong at 7 AM
            RecurringJob.AddOrUpdate<ICbsTransactionImportRepository>(job => job.ImportCbsOutgoingTransaction(), "0 7 * * *"); // Run on the mornong at 7 AM
            RecurringJob.AddOrUpdate<ICbsTransactionImportRepository>(job => job.ImportCbsIncomingTransaction(), "0 7 * * *"); // Run on the mornong at 7 AM
            RecurringJob.AddOrUpdate<ICbsTransactionImportRepository>(job => job.ImportPendingCbsIncomingTransaction(), "*/20 18-23 * * *"); // Run on the mornong at 7 AM
            //Initialize(app.ApplicationServices);
        }
        private static void Initialize(IServiceProvider service)
        {
            using (var serviceScope = service.CreateScope())
            {

                var scopeServiceProvider = serviceScope.ServiceProvider;
               // var db = scopeServiceProvider.GetService<IlriInfomanDBContext>();

            }
        }
       
    }
}