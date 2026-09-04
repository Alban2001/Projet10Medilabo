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

builder.Services.AddAuthorization();

var app = builder.Build();

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
