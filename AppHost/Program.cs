var builder = DistributedApplication.CreateBuilder(args);

// Backing Services 
var postgres = builder.AddPostgres("postgres")
               .WithPgAdmin()
               .WithDataVolume()
               .WithLifetime(ContainerLifetime.Session);

var catalogDb = postgres.AddDatabase("catalogdb");

// Projects
var catalog = builder.AddProject<Projects.Catalog>("catalog")
                     .WithReference(catalogDb)
                     .WaitFor(catalogDb);

builder.Build().Run();
