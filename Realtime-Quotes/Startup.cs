using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealtimeQuotes.Infrastructure.Hubs;
using RealtimeQuotes.Infrastructure.Services;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
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
            services.AddHostedService<QueuedHostedService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<GoogleWeatherQuoteService>();
            services.AddHttpClient("googleWeather", client =>
            {
                client.BaseAddress = new System.Uri("https://weather-fa.azurewebsites.net");
            }).AddTypedClient<GoogleWeatherQuoteService>();


            services.AddSingleton<YahooWeatherQuoteService>();
            services.AddHttpClient("yahooWeather", client =>
            {
                client.BaseAddress = new System.Uri("https://weather-fa.azurewebsites.net");
            }).AddTypedClient<YahooWeatherQuoteService>();

            services.AddSingleton<OpenWeatherQuoteService>();
            services.AddHttpClient("openWeather", client =>
            {
                client.BaseAddress = new System.Uri("https://weather-fa.azurewebsites.net");
            }).AddTypedClient<OpenWeatherQuoteService>();


            services.AddSingleton<WorldWeatherQuoteService>();
            services.AddHttpClient("worldWeather", client =>
            {
                client.BaseAddress = new System.Uri("https://weather-fa.azurewebsites.net");
            }).AddTypedClient<WorldWeatherQuoteService>();
            

            services.AddSingleton<IList<IQuoteService>>(serviceProvider =>
            {

                return new List<IQuoteService>()
                {
                    serviceProvider.GetRequiredService<GoogleWeatherQuoteService>(),
                    serviceProvider.GetRequiredService<YahooWeatherQuoteService>(),
                    serviceProvider.GetRequiredService<OpenWeatherQuoteService>(),
                    serviceProvider.GetRequiredService<WorldWeatherQuoteService>()
                };
            });
            services.AddSignalR().AddAzureSignalR();
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
