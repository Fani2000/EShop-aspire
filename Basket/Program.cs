using ServiceDefault.Messaging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.AddServiceDefaults();
builder.AddKeyedRedisDistributedCache("cache");

builder.Services.AddScoped<BasketService>();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<CatalogApiClient>(client =>
{
    client.BaseAddress = new("https+http://catalog");
});
builder.Services.AddMassTransitWithAssemblies(Assembly.GetExecutingAssembly());
builder.Services.AddAuthentication()
                .AddKeycloakJwtBearer(serviceName: "Keycloak",
                realm: "eshop",
                configureOptions: options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "account";
                });
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapBasketEndpoints();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
