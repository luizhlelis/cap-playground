using DotNetCore.CAP.Internal;
using Microsoft.EntityFrameworkCore;
using Catalog.Consumer;
using Catalog.Consumer.Consumers;
using Catalog.Consumer.Domain.Events;
using Catalog.Consumer.Domain.Services;
using Catalog.Consumer.Idempotency;
using Catalog.Consumer.Infrastructure;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        // Database
        services.AddDbContext<CatalogContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CatalogContext")));

        // Business rules
        services
            .AddScoped<OrderCreatedConsumer>()
            .AddConsumerService<OrderCreated, OrderCreatedService>(
                options =>
                {
                    options.UseIdempotency<CatalogContext>();
                });

        // CAP
        services.AddCap(options =>
            {
                options.UseEntityFramework<CatalogContext>();

                options.DefaultGroupName = "catalog";

                options.UseRabbitMQ(o =>
                {
                    o.HostName = configuration.GetValue<string>("RabbitMQ:HostName");
                    o.UserName = configuration.GetValue<string>("RabbitMq:UserName");
                    o.Password = configuration.GetValue<string>("RabbitMq:Password");
                    o.Port = configuration.GetValue<int>("RabbitMQ:Port");
                    o.ExchangeName = configuration.GetValue<string>("RabbitMQ:ExchangeName");
                    o.VirtualHost = configuration.GetValue<string>("RabbitMq:VHost");

                    o.CustomHeaders = e => new List<KeyValuePair<string, string>>
                    {
                        new(DotNetCore.CAP.Messages.Headers.MessageId,
                            SnowflakeId.Default().NextId().ToString()),
                        new(DotNetCore.CAP.Messages.Headers.MessageName, e.RoutingKey)
                    };
                });
            })
            .AddSubscribeFilter<BootstrapFilter>();

        // OpenTelemetry
        services.AddOpenTelemetryTracing((builder) => builder
            .AddSqlClientInstrumentation(options => options.SetDbStatementForText = true)
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(configuration["Otlp:ServiceName"]))
            .AddCapInstrumentation()
            .AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("Otlp:Endpoint"));
                otlpOptions.Protocol = OtlpExportProtocol.Grpc;
            })
        );

        services.Configure<AspNetCoreInstrumentationOptions>(
            configuration.GetSection("AspNetCoreInstrumentation"));
    })
    .Build();

await host.RunAsync();
