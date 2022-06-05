using DotNetCore.CAP.Internal;
using Microsoft.EntityFrameworkCore;
using Catalog.Consumer;
using Catalog.Consumer.Consumers;
using Catalog.Consumer.Domain.Events;
using Catalog.Consumer.Domain.Services;
using Catalog.Consumer.Infrastructure;
using Ziggurat;
using Ziggurat.CapAdapter;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddDbContext<CatalogContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderContext")));
        services
            .AddScoped<OrderCreatedConsumer>()
            .AddConsumerService<OrderCreated, OrderCreatedService>(
                options =>
                {
                    options.UseIdempotency<CatalogContext>();
                });
        services.AddCap(options =>
            {
                options.UseEntityFramework<CatalogContext>();

                options.DefaultGroupName = "catalog";

                options.UseRabbitMQ(o =>
                {
                    o.HostName = configuration.GetValue<string>("RabbitMQ:HostName");
                    o.Port = configuration.GetValue<int>("RabbitMQ:Port");
                    o.ExchangeName = configuration.GetValue<string>("RabbitMQ:ExchangeName");

                    o.CustomHeaders = e => new List<KeyValuePair<string, string>>
                    {
                        new(DotNetCore.CAP.Messages.Headers.MessageId,
                            SnowflakeId.Default().NextId().ToString()),
                        new(DotNetCore.CAP.Messages.Headers.MessageName, e.RoutingKey)
                    };
                });
            })
            .AddSubscribeFilter<BootstrapFilter>();
    })
    .Build();

await host.RunAsync();
