using Microsoft.AspNetCore.Mvc;
using Roxana.Application.Core.Base;
using Roxana.Application.Core.Enums.Membership;
using Roxana.Application.Core.Models.Membership;

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

                var claims = User.Identities.First().Claims.ToArray();

                if (_currentUser == null)
                {
                    var userType = claims.First(c => c.Type == RoxanaClaims.AuthUserType).Value;
                    var userId = claims.First(c => c.Type == RoxanaClaims.AuthUserId).Value;
                    var username = claims.First(c => c.Type == RoxanaClaims.AuthUserName).Value;
                    Enum.TryParse(userType, true, out UserType role);
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