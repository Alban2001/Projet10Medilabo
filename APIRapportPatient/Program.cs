using APIRapportPatient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient("RapportAPIService", client => { client.BaseAddress = new Uri("http://apigatewaymedilabo:8080"); });

builder.Services.AddScoped<RapportAPIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
