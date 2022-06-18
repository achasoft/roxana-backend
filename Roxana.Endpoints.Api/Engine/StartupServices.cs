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
        services.AddAuthentication();
        services.AddControllers();
            // .AddNewtonsoftJson(options =>
            // {
            //     options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //     options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
            //     options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            //     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //     options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //     options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
            //     options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = null };
            // });
        services.AddRazorPages();
    }
}