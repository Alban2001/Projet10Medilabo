using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "configuration.json",
    optional: false,
    reloadOnChange: true);

builder.Services.AddControllers();

builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("IdentityApiKey", options =>
    {
        options.RequireHttpsMetadata = false;

        // à adapter selon ton service d'authentification
        options.Authority = "http://localhost:32771";
        options.Audience = "gateway";
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerForOcelotUI(opt => {
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();

app.Run();