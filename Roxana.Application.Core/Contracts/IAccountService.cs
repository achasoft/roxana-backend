using System.Security.Claims;
using Roxana.Application.Core.Enums.Membership;

namespace Roxana.Application.Core.Contracts;

public interface IAccountService
{
    ClaimsPrincipal ExtractToken(string token, UserType role);
}