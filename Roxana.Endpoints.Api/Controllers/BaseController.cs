using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Roxana.Application.Core.Enums.Membership;
using Roxana.Application.Core.Models.Membership;
using Roxana.Endpoints.Api.Filters;
using Roxana.Endpoints.Api.Models.Membership;

namespace Roxana.Endpoints.Api.Controllers;
public abstract class BaseController : Controller
{
    private TokenClaimsViewModel? _currentUser;

    protected TokenClaimsViewModel Identity
    {
        get
        {
            try
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated) 
                    return new TokenClaimsViewModel();
                
                if (_currentUser == null)
                {
                    var userId = User.FindFirstValue(RoxanaClaims.UserId);
                    var username = User.FindFirstValue(RoxanaClaims.Username);
                    Enum.TryParse(userId, true, out UserType role);
                    _currentUser = new TokenClaimsViewModel(username, Guid.Parse(userId), role);
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