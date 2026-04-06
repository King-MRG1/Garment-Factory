using Database.Data;
using Database.Models;
//using Database.Data.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Repository.Extensions;
using Scalar.AspNetCore;
using Services.Extensions;
using System.Text;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllers();

            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, token) =>
                {
                    if (document.Components == null)
                    {
                        document.Components = new OpenApiComponents();
                    }

                    // ✅ Initialize SecuritySchemes if null
                    if (document.Components.SecuritySchemes == null)
                    {
                        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
                    }

                    // ✅ Now safely add Bearer scheme
                    document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter your JWT token"
                    });

                    return Task.CompletedTask;
                });
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddRepositories();
            builder.Services.AddServices();
            builder.Services.AddDbContext<AppDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var jwtIssuer = builder.Configuration["JWT:Issuer"];
            var jwtAudience = builder.Configuration["JWT:Audience"];
            var jwtKey = builder.Configuration["JWT:Key"];
            var jwtExpireMinutes = builder.Configuration["JWT:ExpireMinutes"];

            if (string.IsNullOrWhiteSpace(jwtIssuer) || 
                string.IsNullOrWhiteSpace(jwtAudience) || 
                string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException(
                    "JWT configuration is missing. Ensure appsettings.json contains JWT:Issuer, JWT:Audience, and JWT:Key sections.");
            }

            if (jwtKey.Length < 32)
            {
                throw new InvalidOperationException(
                    "JWT:Key must be at least 32 characters long for HS256 algorithm. Current length: " + jwtKey.Length);
            }

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
                .AddJwtBearer("JwtBearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)),
                        
                        
                        ClockSkew = TimeSpan.Zero  
                    };
                });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.MapOpenApi();
            app.MapScalarApiReference();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}
