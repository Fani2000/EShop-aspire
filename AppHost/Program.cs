var builder = DistributedApplication.CreateBuilder(args);

// Backing Services 
var postgres = builder.AddPostgres("postgres")
               .WithPgAdmin()
               .WithLifetime(ContainerLifetime.Session);

var catalogDb = postgres.AddDatabase("catalogdb");

var cache = builder.AddRedis("cache")
                    .WithRedisInsight()
                    .WithLifetime(ContainerLifetime.Session);

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
                       .WithManagementPlugin()
                       .WithLifetime(ContainerLifetime.Session);

var keycloak = builder.AddKeycloak("keycloak", 8080)
               .WithLifetime(ContainerLifetime.Persistent);

// Adding the ollama and model llma3.2
var ollama = builder.AddOllama("ollama", 11434)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithOpenWebUI()
    .WithDataVolume()
    .AddModel("llama3.2");

if(builder.ExecutionContext.IsRunMode)
{
    postgres.WithDataVolume();
    keycloak.WithDataVolume();
    cache.WithDataVolume();
    rabbitMq.WithDataVolume();
}


// Projects
var catalog = builder.AddProject<Projects.Catalog>("catalog")
                     .WithReference(rabbitMq)
                     .WithReference(catalogDb)
                     .WithReference(ollama)
                     .WaitFor(rabbitMq)
                     .WaitFor(ollama)
                     .WaitFor(catalogDb);

var basket = builder.AddProject<Projects.Basket>("basket")
                     .WithReference(rabbitMq)
                     .WithReference(catalog)
                     .WithReference(cache)
                     .WithReference(keycloak)
                     .WaitFor(rabbitMq)
                     .WaitFor(keycloak)
                     .WaitFor(cache);

var vueWebApp = builder.AddNpmApp("vuewebapp", "../vuewebapp")
                       .WithEnvironment("BROWSER", "none")
                       .WithHttpEndpoint(env: "VITE_PORT")
                       .WithExternalHttpEndpoints()
                       .PublishAsDockerFile()
                       .WithReference(catalog)
                       .WaitFor(catalog);

var webapp = builder.AddProject<Projects.WebApp>("webapp")
                    .WithExternalHttpEndpoints()
                    .WithReference(cache)
                    .WithReference(catalog)
                    .WithReference(basket)
                    .WaitFor(catalog)
                    .WaitFor(cache)
                    .WaitFor(basket);


builder.Build().Run();
