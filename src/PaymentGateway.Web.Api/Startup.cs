namespace PaymentGateway.Web.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureDependencies(services);

        services
            .AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddDebug();
                opt.SetMinimumLevel(LogLevel.Trace);
            })
            .AddControllers()
            .AddApplicationPart(typeof(Startup).Assembly)
            .AddControllersAsServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    protected virtual void ConfigureDependencies(IServiceCollection services)
    {
    }
}