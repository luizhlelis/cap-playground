using Microsoft.EntityFrameworkCore;
using Product.Consumer;
using Product.Consumer.Infrastructure;

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
    })
    .Build();

await host.RunAsync();
