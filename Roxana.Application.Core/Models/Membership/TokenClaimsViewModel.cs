using Roxana.Application.Core.Enums.Membership;

namespace Roxana.Application.Core.Models.Membership;

public class TokenClaimsViewModel
{
    public TokenClaimsViewModel(string username, Guid userId, UserType type = UserType.User, DateTime? expires = null)
    {
        IsAuthenticated = true;
        UserId = userId;
        Username = username;
        UserType = type;
        Expires = expires;
    }

    public TokenClaimsViewModel()
    {
        IsAuthenticated = false;
        UserId = Guid.Empty;
        Username = string.Empty;
        UserType = UserType.Anonymous;
    }

    public bool IsAuthenticated { get; private set; }
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public DateTime? Expires { get; set; }
    public UserType UserType { get; set; }
}