using Microsoft.EntityFrameworkCore;
using APIEndpoint.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using APIEndpoint.ModelBinders;

namespace APIEndpoint;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new AuthorModelBinderProvider());
        });
        builder.Services.AddTransient<AuthorModelBinder>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
            new string[] {}
        }
    });
        });
        
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecureSecretKey@2024#Refhub"))
        };
    });


        builder.Services.AddDbContext<RefhubContext>(options =>
            options.UseSqlite("Data Source==../Daneshkar_BC1403_BookStoreMVC/wwwroot/db/refhub.db"));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        // اعمال Routing های سیستم
        app.UseRouting();
        app.UseEndpoints(static endpoints =>
        {
            // مسیر GET برای دریافت همه دسته‌بندی‌ها
            endpoints.MapControllerRoute(
                name: "categories-list",
                pattern: "api/ConventionCategories",
                defaults: new { controller = "ConventionCategories", action = "GetCategories" }
            );

            // مسیر GET برای دریافت یک دسته‌بندی با شناسه
            endpoints.MapControllerRoute(
                name: "category-details",
                pattern: "api/ConventionCategories/{id}",
                defaults: new { controller = "ConventionCategories", action = "GetCategory" }
            );

            // مسیر POST برای ایجاد دسته‌بندی جدید
            endpoints.MapControllerRoute(
                name: "category-create",
                pattern: "api/ConventionCategories",
                defaults: new { controller = "ConventionCategories", action = "PostCategory" }
            );

            // مسیر PUT برای به‌روزرسانی دسته‌بندی
            endpoints.MapControllerRoute(
                name: "category-update",
                pattern: "api/ConventionCategories/{id}",
                defaults: new { controller = "ConventionCategories", action = "PutCategory" }
            );

            // مسیر DELETE برای حذف دسته‌بندی
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
