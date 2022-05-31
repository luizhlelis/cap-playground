using DotNetCore.CAP.Internal;
using Microsoft.EntityFrameworkCore;
using Product.Consumer;
using Product.Consumer.Consumers;
using Product.Consumer.Events;
using Product.Consumer.Infrastructure;
using Product.Consumer.Services;
using Ziggurat;
using Ziggurat.CapAdapter;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddDbContext<ProductContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderContext")));
        services
            .AddScoped<OrderCreatedConsumer>()
            .AddConsumerService<OrderCreated, OrderCreatedService>(
                options =>
                {
                    options.UseIdempotency<ProductContext>();
                });
        services.AddCap(options =>
            {
                options.UseEntityFramework<ProductContext>();

                options.DefaultGroupName = "catalog";

                options.UseRabbitMQ(o =>
                {
                    o.HostName = configuration.GetValue<string>("RabbitMQ:HostName");
                    o.Port = configuration.GetValue<int>("RabbitMQ:Port");
                    o.ExchangeName = configuration.GetValue<string>("RabbitMQ:ExchangeName");

                    o.CustomHeaders = e => new List<KeyValuePair<string, string>>
                    {
                        new(DotNetCore.CAP.Messages.Headers.MessageId, SnowflakeId.Default().NextId().ToString()),
                        new(DotNetCore.CAP.Messages.Headers.MessageName, e.RoutingKey)
                    };
                });
            })
            .AddSubscribeFilter<BootstrapFilter>();
    })
    .Build();

await host.RunAsync();
