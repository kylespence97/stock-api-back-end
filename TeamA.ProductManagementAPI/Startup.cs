using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using TeamA.Data;
using TeamA.Services;

namespace TeamA.ProductManagementAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment Environment;

        // Adds services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ProductManagementDb>(options => options.UseSqlServer(
            Configuration.GetConnectionString("ProductManagementConnection"), optionsBuilder =>
            {
                // Retry pattern for EF SQL
                optionsBuilder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
            }
            ));

            if (Environment.IsDevelopment())
            {
                // Internals
                services.AddScoped<IStockService, StockRepository>();
                services.AddScoped<ICustomersService, CustomersRepository>();
            }
            else
            {
                // Internal
                services.AddScoped<IStockService, StockRepository>();

                // External using Polly
                services.AddScoped<ICustomersService, CustomersRepository>();
                //services.AddHttpClient<ICustomersService, LiveCustomersRepository>(c =>
                //{
                //    c.BaseAddress = new Uri("https://teamacustomeraccounts.azurewebsites.net/");
                //    c.DefaultRequestHeaders.Accept.Clear();
                //}).AddTransientHttpErrorPolicy(p =>
                //     p.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                //        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                //    .AddTransientHttpErrorPolicy(p =>
                //        p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            }

            #region Security
            // AddCors for React
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Audience = "staff_api"; ;
                options.Authority = "https://threeamigosauth.azurewebsites.net/";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Customer", builder =>
                {
                    builder.RequireClaim("role", "Customer", "Admin", "Staff"); // Only Staff and Admin
                });
                options.AddPolicy("Staff", builder =>
                {
                    builder.RequireClaim("role", "Staff", "Admin"); // Only Admin
                });
            });
            #endregion
        }

        // Configures HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
