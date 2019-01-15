using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web
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
             
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(
                  cfg =>
                  {
                      var host = cfg.Host(new Uri(Configuration.GetSection("RabbitMqHost").Value), h =>
                      {
                          h.Username(Configuration.GetSection("RabbitMqUsername").Value);
                          h.Password(Configuration.GetSection("RabbitMqPassword").Value);
                      }); 
                  }));

            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, MassTransitHostedService>();
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
                app.UseExceptionHandler("/Error");
            }


            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
        public class MassTransitHostedService : Microsoft.Extensions.Hosting.IHostedService
        {
            private readonly IBusControl busControl;

            public MassTransitHostedService(IBusControl busControl)
            {
                this.busControl = busControl;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                await busControl.StartAsync(cancellationToken);
            }

            public async Task StopAsync(CancellationToken cancellationToken)
            {
                await busControl.StopAsync(cancellationToken);
            }
        }
    }
}
