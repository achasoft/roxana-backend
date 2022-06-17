using Microsoft.AspNetCore.Mvc;
using Roxana.Application.Core.Enums.Membership;

namespace Roxana.Endpoints.Api.Filters
{
    public class RoxanaAuthorizeAttribute : TypeFilterAttribute
    {
        public RoxanaAuthorizeAttribute(UserType accountType = UserType.User)
            : base(typeof(RoxanaAuthorize))
        {
            Arguments = new object[] {accountType};
        }
    }
}