using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Roxana.Application.Core.Enums.Membership;
using Roxana.Application.Core.Models.Membership;
using Roxana.Endpoints.Api.Filters;
using Roxana.Endpoints.Api.Models.Membership;

namespace Roxana.Endpoints.Api.Controllers;

[Localize]
public abstract class BaseController : Controller
{
    private TokenClaimsViewModel _currentUser;

    protected TokenClaimsViewModel Identity
    {
        get
        {
            try
            {
                if (User == null || !User.Identity.IsAuthenticated) return new TokenClaimsViewModel();
                if (_currentUser == null)
                {
                    Enum.TryParse(User.FindFirstValue(RoxanaClaims.UserId), true, out UserType role);
                    _currentUser = new TokenClaimsViewModel(
                        User.FindFirstValue(RoxanaClaims.Username),
                        Guid.Parse(User.FindFirstValue(RoxanaClaims.UserId)),
                        role
                    );
                }

                return _currentUser;
            }
            catch
            {
                return null;
            }
        }
    }
}