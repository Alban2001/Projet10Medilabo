using Data;
using Models;
using APIInformationsPatient.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// REPOSITORIES
// ======================================================

builder.Services.AddScoped<IPatientRepository, PatientRepository>();

// ======================================================
// DATABASE - SQL SERVER
// ======================================================

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            // Permet de réessayer automatiquement si SQL Server
            // met quelques secondes à être complètement disponible.
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
        });
});

// ======================================================
// JWT AUTHENTICATION
// ======================================================

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        // HTTPS non obligatoire dans l'environnement Docker
        // de développement.
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ClockSkew = TimeSpan.Zero,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    )
            };
    });

// ======================================================
// AUTHORIZATION
// ======================================================

builder.Services.AddAuthorization();

// ======================================================
// CONTROLLERS
// ======================================================

builder.Services.AddControllers();

// ======================================================
// SWAGGER
// ======================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "API : Informations Patient",
            Version = "v1"
        }
    );

    // ----------------------------------------------
    // JWT Bearer
    // ----------------------------------------------

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,

            Description =
                "Veuillez entrer un jeton JWT valide.",

            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            BearerFormat = "JWT",

            Scheme = "Bearer"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        }
    );
});

// ======================================================
// BUILD APPLICATION
// ======================================================

var app = builder.Build();

// ======================================================
// DATABASE INITIALIZATION
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext =
            services.GetRequiredService<AppDBContext>();

        // --------------------------------------------------
        // Application des migrations EF Core
        // --------------------------------------------------
        //
        // Si la base existe déjà et que les migrations sont
        // à jour, aucune modification ne sera effectuée.
        //
        // Si une nouvelle migration existe, elle sera appliquée.
        // --------------------------------------------------

        await dbContext.Database.MigrateAsync();

        Console.WriteLine(
            "Base de données DBPatientMedilabo initialisée avec succès."
        );
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "=================================================="
        );

        Console.WriteLine(
            "ERREUR LORS DE L'INITIALISATION DE LA BASE"
        );

        Console.WriteLine(
            "=================================================="
        );

        Console.WriteLine(ex.Message);

        if (ex.InnerException != null)
        {
            Console.WriteLine(
                $"Exception interne : {ex.InnerException.Message}"
            );
        }

        // L'API continue de démarrer.
        // Cela évite qu'un problème temporaire avec SQL Server
        // empêche complètement le conteneur de démarrer.
    }
}

// ======================================================
// MIDDLEWARES
// ======================================================

app.UseSwagger();

app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// ======================================================
// START APPLICATION
// ======================================================

app.Run();