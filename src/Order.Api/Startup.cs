using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;
using Order.Api.Infrastructure;

namespace Order.Api;

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

        // Swagger
        var openApiInfo = new OpenApiInfo();
        Configuration.Bind("OpenApiInfo", openApiInfo);
        services.AddSingleton(openApiInfo);
        services.AddSwaggerGen();

        // Databases
        services.AddDbContext<OrderContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("OrderContext")));
        
        // CAP
        services.AddCap(x =>
        {
            x.UseEntityFramework<OrderContext>();

            x.UseRabbitMQ(o =>
            {
                o.HostName = Configuration.GetValue<string>("RabbitMq:HostName");
                o.Port = Configuration.GetValue<int>("RabbitMq:Port");
                o.ExchangeName = Configuration.GetValue<string>("RabbitMq:ExchangeName");
            });
        });
        

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
                c.SwaggerEndpoint(
                    $"/swagger/v1/swagger.json",
                    "v1")
        );

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
