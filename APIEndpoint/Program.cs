using Microsoft.EntityFrameworkCore;
using APIEndpoint.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using APIEndpoint.ModelBinders;
using API.Models;
using Microsoft.AspNetCore.Identity;

namespace APIEndpoint;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<RefhubContext>(options =>
            options.UseSqlite("Data Source=../Daneshkar_BC1403_BookStoreMVC/wwwroot/db/refhub.db"));

        // Configure Identity - MUST be before Authentication
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<long>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<RefhubContext>()
        .AddDefaultTokenProviders();

        // Configure Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://yourdomain.com",
                ValidAudience = "https://yourdomain.com",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("YourSuperSecureSecretKey@2024#Refhub"))
            };
        });

        builder.Services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new AuthorModelBinderProvider());
        });

        builder.Services.AddTransient<AuthorModelBinder>();

        // Configure Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Endpoint", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "برای ورود، مقدار 'Bearer YOUR_TOKEN' را وارد کنید."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Order matters for these middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Configure routing
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "categories-list",
                pattern: "api/ConventionCategories",
                defaults: new { controller = "ConventionCategories", action = "GetCategories" }
            );

            endpoints.MapControllerRoute(
                name: "category-details",
                pattern: "api/ConventionCategories/{id}",
                defaults: new { controller = "ConventionCategories", action = "GetCategory" }
            );

            endpoints.MapControllerRoute(
                name: "category-create",
                pattern: "api/ConventionCategories",
                defaults: new { controller = "ConventionCategories", action = "PostCategory" }
            );

            endpoints.MapControllerRoute(
                name: "category-update",
                pattern: "api/ConventionCategories/{id}",
                defaults: new { controller = "ConventionCategories", action = "PutCategory" }
            );

            endpoints.MapControllerRoute(
                name: "category-delete",
                pattern: "api/ConventionCategories/{id}",
                defaults: new { controller = "ConventionCategories", action = "DeleteCategory" }
            );
        });

        app.MapControllers();

        app.Run();
    }
}