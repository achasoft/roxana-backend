using Roxana.Application.Core.Enums.Membership;

namespace Roxana.Application.Core.Models.Membership;

public class AccountProfileDto
{
    public string Timezone { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public UserType UserType { get; set; }
    public string FullName { get; set; }
}