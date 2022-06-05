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

> **NOTE:** the command above will up the web application, SqlServer and will execute all the existing migrations.
> That's enough to run everything, but even though, if you want to run the app locally using containerized dependencies,
> you must try the commands below.

To run the SqlServer, type the following command in
the [src](./src) folder:

```shell
docker-compose up sql-server-database
```

To up the existent migrations in SqlServer, type the following command in
the [src](./src) folder:

```shell
docker-compose up --build migrations
```

If you want to create a new migration (after an entity model update, for example), type the following command in
the [root](./) folder:

```shell
dotnet ef migrations add <migration-name> --project src/Order.Api/Order.Api.csproj --startup-project src/Order.Api/Order.Api.csproj --context OrderContext --verbose

dotnet ef migrations add InitialMigration --project src/Catalog.Consumer/Catalog.Consumer.csproj --startup-project src/Catalog.Consumer/Catalog.Consumer.csproj --context CatalogContext --verbose
```

To run all the automated test, type the following command in the [src](./src) folder:

```shell
dotnet test
```

If you're running the app in docker, open the following link in your browser:

```shell
http://localhost/swagger/index.html
```

Otherwise, if you're running it locally, the port will be different:

```shell
https://localhost:7211/swagger/index.html
```

Open a browser and go to `http://localhost:16686/` to see the traces in `jaeger`.