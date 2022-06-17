namespace Roxana.Endpoints.Api.Engine;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        StartupServices.Register(services);
        Application.Business.StartupServices.Register(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // app.UseCookiePolicy(new CookiePolicyOptions
        // {
        //     MinimumSameSitePolicy = SameSiteMode.Lax,
        //     Secure = CookieSecurePolicy.SameAsRequest,
        //     CheckConsentNeeded = context => true
        // });
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v2/swagger.json", "Roxana Api");
            c.RoutePrefix = "";
            c.DocumentTitle = "Roxana Api";
        });
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}