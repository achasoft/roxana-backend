namespace Roxana.Application.Core.Models.Membership;

public class AccountSigninResponseDto
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
}
public class AccountSigninRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}