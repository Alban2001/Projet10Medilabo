using FrontPatient.Data;
using FrontPatient.Services;
using Microsoft.AspNetCore.Authentication;
using NuGet.Common;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("HistoriqueAPIService", client => { client.BaseAddress = new Uri("http://apigatewaymedilabo:8080"); });
builder.Services.AddHttpClient("PatientAPIService", client => { client.BaseAddress = new Uri("http://apigatewaymedilabo:8080"); });

builder.Services.AddScoped<PatientAPIService>();
builder.Services.AddScoped<HistoriqueAPIService>();
builder.Services.AddScoped<DbSeeder>();

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=Index}/{id?}");

app.Run();
