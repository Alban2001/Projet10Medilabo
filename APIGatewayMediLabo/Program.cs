using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "configuration.json",
    optional: false,
    reloadOnChange: true);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("IdentityApiKey", options =>
    {
        options.RequireHttpsMetadata = false;

        // à adapter selon ton service d'authentification
        options.Authority = "http://localhost:5000";
        options.Audience = "api";
    });

builder.Services.AddOcelot();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();