using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
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
                o.UserName = Configuration.GetValue<string>("RabbitMq:UserName");
                o.Password = Configuration.GetValue<string>("RabbitMq:Password");
                o.Port = Configuration.GetValue<int>("RabbitMq:Port");
                o.ExchangeName = Configuration.GetValue<string>("RabbitMq:ExchangeName");
                o.VirtualHost = Configuration.GetValue<string>("RabbitMq:VHost");
            });
        });

        // OpenTelemetry
        services.AddOpenTelemetryTracing((builder) => builder
            .AddAspNetCoreInstrumentation()
            .AddSqlClientInstrumentation(options => options.SetDbStatementForText = true)
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(Configuration["Otlp:ServiceName"]))
            .AddCapInstrumentation()
            .AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(Configuration.GetValue<string>("Otlp:Endpoint"));
                otlpOptions.Protocol = OtlpExportProtocol.Grpc;
            })
        );

        services.Configure<AspNetCoreInstrumentationOptions>(
            Configuration.GetSection("AspNetCoreInstrumentation"));

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", openApiInfo);
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint(
                $"/swagger/v1/swagger.json",
                "Order.Api v1")
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
