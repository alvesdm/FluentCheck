using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentCheck.HealthCheck;
using FluentCheck.HealthCheck.RabbitMQHealthCheck;
using FluentCheck.HealthCheck.UrlHealthCheck;
using FluentCheck.HealthCheck.WorkerHealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FluentCheck.Test
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
            services.AddTransient<IRabbitMQHealthCheck, RabbitMQHealthCheck>();
            services.AddTransient<IUrlHealthCheck, UrlHealthCheck>();
            services.AddTransient<IWorkerHealthCheck, WorkerHealthCheck>();
            services.AddHealthCheck(); //see Program.cs

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthCheck(
                serviceProvider,
                config =>
                    config
                        .Register<IRabbitMQHealthCheck>("RabbitMQ", hc => hc.WithCredentials("guest", "guest"))
                        .Register<IUrlHealthCheck>("Google", hc => hc.WithAddress("http://google.com"))
                        .Register<IWorkerHealthCheck>("Worker", hc => hc.WithPingAddress("myworker/ping"))
                        .WithEndpoint("/healthcheck")
            );

            app.UseMvc();
        }
    }
}
