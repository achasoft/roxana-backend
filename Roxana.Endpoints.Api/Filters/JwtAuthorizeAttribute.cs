using Microsoft.AspNetCore.Mvc;
using Roxana.Application.Core.Enums.Membership;

namespace Roxana.Endpoints.Api.Filters
{
    public class JwtAuthorizeAttribute : TypeFilterAttribute
    {
        public JwtAuthorizeAttribute(UserType accountType = UserType.User)
            : base(typeof(JwtAuthorize))
        {
            Arguments = new object[] {accountType};
        }
    }
}