using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentCheck.HealthCheck;
using FluentCheck.HealthCheck.RabbitMQHealthCheck;
using FluentCheck.HealthCheck.UrlHealthCheck;
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
            services.AddHealthCheck();

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
                        .Register<IRabbitMQHealthCheck>("RabbitMQ", hc => hc.WithCredentials("guestt", "guest"))
                        .Register<IUrlHealthCheck>("Google", hc => hc.WithAddress("http://google.comm"))
                        .WithEndpoint("/healthcheck")
            );

            app.UseMvc();
        }
    }
}
