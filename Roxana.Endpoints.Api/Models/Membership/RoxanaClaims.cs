using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Roxana.Endpoints.Api.Models.Membership;

public static class RoxanaClaims
{
    public const string TokenId = JwtRegisteredClaimNames.Jti;
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string Username = JwtRegisteredClaimNames.Sub;
    public const string UserType = "usertype";
}