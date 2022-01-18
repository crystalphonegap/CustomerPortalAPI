using CustomerPortalWebApi.Context;
using CustomerPortalWebApi.Helper;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Security;
using CustomerPortalWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SapNwRfc.Pooling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustomerPortalWebApi
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
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            JwtSettings settings = GetJwtSettings();
            services.AddSingleton<JwtSettings>(settings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(settings.MinToExpiration)
                };
            });

            services.AddControllers();

            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DatabaseContext")));

            //For all to Access WebApi

            //Commented By Ahmed on 20-11-2020
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("EnableCORS", builder =>
            //        builder.SetIsOriginAllowed(_ => true)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});

            services.AddCors(options =>
            { 
                options.AddPolicy("EnableCORS", builder => 
                 builder.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });


            //Commented By SUman on 20-11-2020
            //added by suman on 20-11-2020 for CORS

            // services.AddCors(options =>
            // {
            //    options.AddPolicy("EnableCORS", builder =>
            //    {
            //        builder.WithOrigins("http://192.168.1.101:4200/");
            //    });
            // });
            //added by suman on 20-11-2020 for CORS

            //for Specific Server to  Access Api
            //services.AddCors(option => option.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
            // For Showing Data as it in from Model Class
            services.AddControllers()
             .AddNewtonsoftJson(options =>
             {
                 options.UseMemberCasing();
             });
            services.AddScoped<IUserMasterService, UserMasterService>();
            services.AddScoped<ICustomerPortalHelper, CustomerPortalHelper>();
            services.AddScoped<ICustomerMasterService, CustomerMasterService>();
            services.AddScoped<IDeliveryOrderService, DeliveryOrderService>();
            services.AddScoped<IInvoiceMasterService, InvoiceMasterService>();
            services.AddScoped<IPaymentReceiptService, PaymentReceiptService>();
            services.AddScoped<ISalesOrderService, SalesOrderService>();
            services.AddScoped<IOutstandingService, OutstandingService>();
            services.AddScoped<ICreditlimitService, CreditlimitService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ITargetSalesService, TargetSalesService>();
            services.AddScoped<IItemMasterService, ItemMasterService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPriceApproval, PriceApprovalService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUploadEmployeeService, UploadEmployeeService>();
            services.AddScoped<ICFAgentServices, CFAgentServices>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IBroadCastservice, BroadCastservice>();
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<ISalesPromoterTargetDataServices, SalesPromoterTargetDataServices>();
            services.AddScoped<IRetailOrderService, RetailOrderService>();
            services.AddScoped<IBalanceConfirmationService, BalanceConfirmationService>();
            services.AddScoped<IRFCCallServices, RFCCallServices>();
            services.AddScoped<Interface.ILogger, Logger>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IChecktokenservice, Checktokenservice>();
            services.AddScoped<ILoginReportService, LoginReportService>();
            services.AddScoped<IEscalationService, EscalationService>();
            services.AddScoped<ILoyalityPointsService, LoyalityPointsService>();
            services.AddScoped<IMaterialTestCertificateService, MaterialTestCertificateService>();
            services.AddScoped<IUploadCustomerevent , UploadCustomereventService>();
            //services.AddScoped<ILoggerManager, LoggerManager>();

            //string sapconnectionstring = "AppServerHost = 172.20.1.20; SystemNumber = 00; User = SAPSUPPORT; Password = Crystal@20#; Client = 700; Language = EN; PoolSize = 5; Trace = 3";

            //string sapconnectionstring = "AppServerHost = 172.20.1.26; SystemNumber = 03; User = SAPSUPPORT; Password = Welcome@123; Client = 700; Language = EN; PoolSize = 5; Trace = 3";

            services.AddSingleton<ISapConnectionPool>(_ => new SapConnectionPool(Configuration["ConnectionStrings:SAPConnectionString"]));
            services.AddScoped<ISapPooledConnection, SapPooledConnection>();
            //services.AddTransient<IMailService, MailService>();

             

        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Error Handling
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //app.ConfigureCustomExceptionMiddleware();

            //added for preventing  Clickjacking Possible by suman on 20-11-2020
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            //    await next();
            //});
            //added for preventing  Clickjacking Possible by suman on 20-11-2020

            //added for preventing Server Banner Discloser by suman on 20-11-2020
            //app.Use(async (context, nextMiddleware) =>
            //{
            //    context.Response.OnStarting(() =>
            //    {
            //        context.Response.Headers.Add("Site", "Simple-Talk");
            //        return Task.FromResult(0);
            //    });
            //    await nextMiddleware();
            //});

            //app.Use(async (context, nextMiddleware) =>
            //{
            //    using (var memory = new MemoryStream())
            //    {
            //        var originalStream = context.Response.Body;
            //        context.Response.Body = memory;

            //        await nextMiddleware();

            //        memory.Seek(0, SeekOrigin.Begin);
            //        var content = new StreamReader(memory).ReadToEnd();
            //        memory.Seek(0, SeekOrigin.Begin);

            //        // Apply logic here for deciding which headers to add
            //        context.Response.Headers.Add("Body", content);

            //        await memory.CopyToAsync(originalStream);
            //        context.Response.Body = originalStream;
            //    }
            //});
            //added for preventing Server Banner Discloser by suman on 20-11-2020

            /*security configuration*/
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(opts => opts.EnabledWithBlockMode());
            app.UseXfo(opts => opts.Deny());
            app.UseCsp(opt => opt
                .BlockAllMixedContent()
                //.StyleSources(x => x.Self().CustomSources("http://fonts.googleapis.com"))
                .StyleSources(x => x.Self().UnsafeInline())
                .ScriptSources(x => x.Self().UnsafeInline())
                .FontSources(x => x.Self())
                .FormActions(x => x.Self())
                .FrameAncestors(x => x.Self())
                .ImageSources(x => x.Self().CustomSources("data:"))
            );
            /*security configuration*/

            app.UseHttpsRedirection();

            
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
                RequestPath = new PathString("/Uploads")
            });

            
            app.UseRouting();

            app.UseCors("EnableCORS");

            //app.UseCors();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            app.UseAuthentication();

            app.UseAuthorization();
            

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
                RequestPath = new PathString("/Uploads")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }

        public JwtSettings GetJwtSettings()
        {
            JwtSettings settings = new JwtSettings();
            settings.Key = Configuration["JwtSettings:key"];
            settings.Issuer = Configuration["JwtSettings:issuer"];
            settings.Audience = Configuration["JwtSettings:audience"];
            settings.MinToExpiration = Convert.ToInt32(Configuration["JwtSettings:minToExpiration"]);
            return settings;
        }
    }
}