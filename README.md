# cap-playground

📤 Order API - Just playing a bit with CAP and outbox pattern

# Pre requisites

- [.NET 6.0](https://dotnet.microsoft.com/en-us/download): to run, build, test the project locally;
- [dotnet-ef tool](https://docs.microsoft.com/en-us/ef/core/cli/dotnet): to create new migrations;
- [Docker](https://www.docker.com/products/docker-desktop/): to run everything inside containers.

# Running commands

To up all the app and dependencies containerized, type the following command in
the [src](./src) folder:

```shell
docker-compose up --build
```

> **NOTE:** the command above will up the web API, the consumer and all dependencies.
> That's enough to run everything, but even though, if you want to run them separately,
> you must try the commands below.

To run all dependencies, type the following command in
the [src](./src) folder:

```shell
docker-compose up sql-server-database rabbit otel-collector migrations
```

To up the API and the consumer, type the following command in
the [src](./src) folder:

```shell
docker-compose up --build order-api catalog-consumer
```

If you want to create a new migration (after an entity model update, for example), type the following command in
the [root](./) folder:

```shell
dotnet ef migrations add <migration-name> --project src/Order.Api/Order.Api.csproj --startup-project src/Order.Api/Order.Api.csproj --context OrderContext --verbose

dotnet ef migrations add <migration-name> --project src/Catalog.Consumer/Catalog.Consumer.csproj --startup-project src/Catalog.Consumer/Catalog.Consumer.csproj --context CatalogContext --verbose
```

If you're running the app in docker, open the following link in your browser:

```shell
http://localhost/swagger/index.html
```

Otherwise, if you're running it locally, the port will be different:

```shell
https://localhost:7259/swagger/index.html
```

Open a browser and go to `http://localhost:16686/` to see the traces in `jaeger`.