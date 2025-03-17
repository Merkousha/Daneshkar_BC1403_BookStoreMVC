using Microsoft.EntityFrameworkCore;
using APIEndpoint.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using APIEndpoint.ModelBinders;
using API.Models;
using Microsoft.AspNetCore.Identity;
using APIEndpoint.Services;

namespace APIEndpoint;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddDbContext<RefhubContext>(options =>
            options.UseSqlite("Data Source=../Daneshkar_BC1403_BookStoreMVC/wwwroot/db/refhub.db"));

        // Configure Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<long>>()
            .AddEntityFrameworkStores<RefhubContext>()
            .AddDefaultTokenProviders();

        // Configure Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://Refhub.ir",
                    ValidAudience = "https://Refhub.ir",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("YourSuperSecretKeySalamSalamChetorid?:)))@2024#Refhub"))
                };
            });

        builder.Services.AddControllers();
        builder.Services.AddTransient<AuthorModelBinder>();
        builder.Services.AddScoped<JwtService>();

        // Configure Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Endpoint", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter 'Bearer' [space] and your token"
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
                    new string[] {}
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Important: Keep this exact order
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
       
        app.Run();
    }
}