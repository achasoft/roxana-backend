using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Roxana.Endpoints.Api.Engine;

public static class StartupServices
{
    public static void Register(IServiceCollection services)
    {
        var issuer = Environment.GetEnvironmentVariable("APP_AUTH_ISSUER") ?? "";
        var secret = Environment.GetEnvironmentVariable("APP_AUTH_SECRET") ?? "";
        
        var key = Encoding.UTF8.GetBytes(secret);
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        
        services.AddSwaggerGen(c =>
        {
            c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
            c.TagActionsBy(api => new List<string> {api.GroupName ?? ""});
            c.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "Roxana Api",
                Version = "v2",
                Description = "Roxana application api explorer"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
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
                    new string[] { }
                }
            });
        });
        
        services.Configure<IdentityOptions>(IdentityConfiguration.ConfigureOptions);
        services
            .AddAuthentication();
            // .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options =>
            // {
            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuer = true,
            //         ValidateAudience = true,
            //         ValidateLifetime = true,
            //         ValidateIssuerSigningKey = true,
            //         ValidIssuer = issuer,
            //         ValidAudience = issuer,
            //         IssuerSigningKey = new SymmetricSecurityKey(key)
            //     };
            // });

        services.AddControllers();
        services.AddRazorPages();
    }
}