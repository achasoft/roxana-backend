using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Roxana.Application.Core.Contracts;
using Roxana.Application.Core.Enums.Membership;

namespace Roxana.Endpoints.Api.Filters;

public class RoxanaAuthorize : IAsyncAuthorizationFilter
{
    private readonly UserType _role;

    public RoxanaAuthorize(UserType role)
    {
        _role = role;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var roles = context.Filters
            .Where(f => f is RoxanaAuthorize)
            .Cast<RoxanaAuthorize>()
            .Select(j => j._role)
            .ToArray();

        if (roles.Contains(UserType.Anonymous))
            // SKIP Authorization
            return;

        var denied = new UnauthorizedResult();
        string token = context.HttpContext.Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
        {
            context.Result = denied;
            return;
        }

        var accountBiz = context.HttpContext.RequestServices.GetService<IAccountService>()!;
        var principal = await accountBiz.ValidateToken(token, _role);
        if (principal == null)
        {
            context.Result = denied;
            return;
        }

        context.HttpContext.User = principal;
        //await context.HttpContext.SignInAsync(principal);
    }
}