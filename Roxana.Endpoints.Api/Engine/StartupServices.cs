using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Roxana.Endpoints.Api.Engine;

public static class StartupServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
            c.TagActionsBy(api => new List<string> {api.GroupName ?? ""});
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Roxana Api",
                Version = "v1",
                Description = "Roxana application api explorer"
            });
        });

        services.Configure<IdentityOptions>(IdentityConfiguration.ConfigureOptions);
        services.AddAuthentication();
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
                // options.SerializerSettings.ContractResolver = new DefaultContractResolver {NamingStrategy = null};
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });
        services.AddRazorPages();
    }
}