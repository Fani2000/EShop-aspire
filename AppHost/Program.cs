using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

var builder = DistributedApplication.CreateBuilder(args);

// Backing Services 
var postgres = builder.AddPostgres("postgres")
               .WithPgAdmin()
               .WithDataVolume()
               .WithLifetime(ContainerLifetime.Session);

var catalogDb = postgres.AddDatabase("catalogdb");

var cache = builder.AddRedis("cache")
                    .WithRedisInsight()
                    .WithDataVolume()
                    .WithLifetime(ContainerLifetime.Session);

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
                       .WithManagementPlugin()
                       .WithDataVolume()
                       .WithLifetime(ContainerLifetime.Session);

var keycloak = builder.AddKeycloak("keycloak", 8080)
               .WithDataVolume()
               .WithLifetime(ContainerLifetime.Persistent);

// Projects
var catalog = builder.AddProject<Projects.Catalog>("catalog")
                     .WithReference(rabbitMq)
                     .WithReference(catalogDb)
                     .WaitFor(rabbitMq)
                     .WaitFor(catalogDb);

var basket = builder.AddProject<Projects.Basket>("basket")
                     .WithReference(rabbitMq)
                     .WithReference(catalog)
                     .WithReference(cache)
                     .WithReference(keycloak)
                     .WaitFor(rabbitMq)
                     .WaitFor(keycloak)
                     .WaitFor(cache);

builder.Build().Run();
