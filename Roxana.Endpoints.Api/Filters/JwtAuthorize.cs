using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Roxana.Application.Core.Contracts;
using Roxana.Application.Core.Enums.Membership;

namespace Roxana.Endpoints.Api.Filters
{
    public class JwtAuthorize : IAuthorizationFilter
    {
        private readonly UserType _role;

        public JwtAuthorize(UserType role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roles = context.Filters.Where(f => f is JwtAuthorize)
                .Cast<JwtAuthorize>()
                .Select(j => j._role)
                .ToArray();
            
            if (roles.Contains(UserType.Anonymous))
            {
                // SKIP Authorization
                return;
            }
            
            var denied = new UnauthorizedResult();
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token)) token = context.HttpContext.Request.Cookies["Authorization"];
            
            if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
            {
                context.Result = denied;
                return;
            }

            var accountBiz = context.HttpContext.RequestServices.GetService<IAccountService>();
            var principal = accountBiz.ExtractToken(token, _role);
            if (principal == null)
            {
                context.Result = denied;
                return;
            }

            context.HttpContext.SignInAsync(principal);
        }
    }
}