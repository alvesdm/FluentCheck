# FluentCheck
A HealthCheck wrapper for .net core


Nuget
```
Install-Package alvesdm.FluentCheck
```

How to use it?


```csharp
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
```

...and of course you can create your own checking stuff.

# License

Code released under the MIT license.



