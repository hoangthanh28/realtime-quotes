using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealtimeQuotes.Infrastructure.Hubs;
using RealtimeQuotes.Infrastructure.Services;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using StackExchange.Redis;
using System.Collections.Generic;

namespace Realtime_Quotes
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddSingleton<IPublisher, QuotePublisher>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<AEQuoteService>();
            services.AddSingleton<CR8QuoteService>();
            services.AddSingleton<CTQuoteService>();
            services.AddSingleton<HCQuoteService>();
            services.AddSingleton<RCQuoteService>();
            services.AddSingleton<IList<IQuoteService>>(serviceProvider =>
            {

                return new List<IQuoteService>()
                {
                    serviceProvider.GetRequiredService<AEQuoteService>(),
                    serviceProvider.GetRequiredService<CR8QuoteService>(),
                    serviceProvider.GetRequiredService<CTQuoteService>(),
                    serviceProvider.GetRequiredService<HCQuoteService>(),
                    serviceProvider.GetRequiredService<RCQuoteService>()
                };
            });
            services.AddHttpClient("ae", client =>
            {
                client.BaseAddress = new System.Uri(Configuration["AppSettings:Endpoint"]);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "jWhDkSCe8BBudvVxKt/Q4g==");

            });
            services.AddHttpClient("hc", client =>
            {
                client.BaseAddress = new System.Uri(Configuration["AppSettings:Endpoint"]);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "jWhDkSCe8BBudvVxKt/Q4g==");

            });
            services.AddHttpClient("ct", client =>
            {
                client.BaseAddress = new System.Uri(Configuration["AppSettings:Endpoint"]);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "jWhDkSCe8BBudvVxKt/Q4g==");

            });
            services.AddHttpClient("rc", client =>
            {
                client.BaseAddress = new System.Uri(Configuration["AppSettings:Endpoint"]);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "jWhDkSCe8BBudvVxKt/Q4g==");

            });
            services.AddHttpClient("cr8", client =>
            {
                client.BaseAddress = new System.Uri(Configuration["AppSettings:Endpoint"]);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "jWhDkSCe8BBudvVxKt/Q4g==");

            });
            services.AddSignalR().AddAzureSignalR();
            services.AddSingleton<ConnectionMultiplexer>(service =>
            {
                string cacheConnection = Configuration.GetConnectionString("RedisCache");
                return ConnectionMultiplexer.Connect(cacheConnection);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseAzureSignalR(routes => routes.MapHub<SearchHub>("/searchroom"));
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
