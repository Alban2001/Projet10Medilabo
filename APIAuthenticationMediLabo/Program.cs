using APIAuthenticationMediLabo.Data;
using APIAuthenticationMediLabo.Models;
using APIAuthenticationMediLabo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// CONTROLLERS
// ======================================================

builder.Services.AddControllers();

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
// IDENTITY
// ======================================================

builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        // ------------------------------
        // Utilisateur
        // ------------------------------

        options.User.RequireUniqueEmail = true;

        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

        // ------------------------------
        // Mot de passe
        // ------------------------------

        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 12;

        // ------------------------------
        // Verrouillage
        // ------------------------------

        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(10);
    })
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

// ======================================================
// JWT SERVICE
// ======================================================

builder.Services.AddScoped<JwtService>();

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

        // En développement Docker
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
// BUILD APPLICATION
// ======================================================

var app = builder.Build();

// ======================================================
// INITIALISATION DE LA BASE DE DONNÉES
// ======================================================
//
// Cette partie est exécutée au démarrage de l'API.
// Elle attend que SQL Server soit réellement disponible
// avant d'effectuer les opérations Identity.
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext =
            services.GetRequiredService<AppDBContext>();

        // --------------------------------------------------
        // Vérification / création de la base via EF
        // --------------------------------------------------

        // Applique les migrations disponibles.
        // Si ta base existe déjà et que les migrations sont
        // à jour, aucune modification ne sera effectuée.
        await dbContext.Database.MigrateAsync();

        // --------------------------------------------------
        // Récupération des services Identity
        // --------------------------------------------------

        var userManager =
            services.GetRequiredService<UserManager<User>>();

        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        // ==================================================
        // CRÉATION DES RÔLES
        // ==================================================

        const string roleAdmin = "ADMIN";
        const string roleUser = "USER";

        // ------------------------------
        // Rôle ADMIN
        // ------------------------------

        if (!await roleManager.RoleExistsAsync(roleAdmin))
        {
            var result =
                await roleManager.CreateAsync(
                    new IdentityRole(roleAdmin)
                );

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(
                        $"Erreur création rôle ADMIN : {error.Description}"
                    );
                }
            }
        }

        // ------------------------------
        // Rôle USER
        // ------------------------------

        if (!await roleManager.RoleExistsAsync(roleUser))
        {
            var result =
                await roleManager.CreateAsync(
                    new IdentityRole(roleUser)
                );

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(
                        $"Erreur création rôle USER : {error.Description}"
                    );
                }
            }
        }

        // ==================================================
        // CRÉATION DU COMPTE ADMINISTRATEUR
        // ==================================================

        const string adminEmail = "admin@medilabo.fr";
        const string adminPassword = "Admin126754?!";

        // Vérifie si l'administrateur existe déjà.
        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);

        // --------------------------------------------------
        // Création uniquement s'il n'existe pas
        // --------------------------------------------------

        if (adminUser == null)
        {
            var newAdmin = new User
            {
                Nom = "VOIRIOT",
                Prenom = "Alban",

                UserName = "admin",

                Email = adminEmail,
                EmailConfirmed = true,

                PhoneNumberConfirmed = true,

                TwoFactorEnabled = false,

                LockoutEnabled = false,

                AccessFailedCount = 0
            };

            var createResult =
                await userManager.CreateAsync(
                    newAdmin,
                    adminPassword
                );

            if (createResult.Succeeded)
            {
                Console.WriteLine(
                    "Utilisateur administrateur créé avec succès."
                );

                // ------------------------------------------
                // Ajout du rôle ADMIN
                // ------------------------------------------

                var roleResult =
                    await userManager.AddToRoleAsync(
                        newAdmin,
                        roleAdmin
                    );

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine(
                            $"Erreur ajout rôle ADMIN : {error.Description}"
                        );
                    }
                }
                else
                {
                    Console.WriteLine(
                        "Rôle ADMIN attribué avec succès."
                    );
                }
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine(
                        $"Erreur création compte admin : {error.Description}"
                    );
                }
            }
        }
        else
        {
            Console.WriteLine(
                "L'utilisateur administrateur existe déjà."
            );

            // --------------------------------------------------
            // Vérification que l'utilisateur possède ADMIN
            // --------------------------------------------------

            if (!await userManager.IsInRoleAsync(
                adminUser,
                roleAdmin))
            {
                var roleResult =
                    await userManager.AddToRoleAsync(
                        adminUser,
                        roleAdmin
                    );

                if (roleResult.Succeeded)
                {
                    Console.WriteLine(
                        "Rôle ADMIN ajouté à l'utilisateur existant."
                    );
                }
            }
        }
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

        // On affiche également l'exception interne si elle existe.
        if (ex.InnerException != null)
        {
            Console.WriteLine(
                $"Exception interne : {ex.InnerException.Message}"
            );
        }

        // --------------------------------------------------
        // IMPORTANT :
        // On ne fait pas planter l'API ici.
        // L'application peut quand même démarrer.
        // --------------------------------------------------
    }
}

// ======================================================
// MIDDLEWARES
// ======================================================

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// ======================================================
// START APPLICATION
// ======================================================

app.Run();