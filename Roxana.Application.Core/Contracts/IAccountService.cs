using System.Security.Claims;
using Roxana.Application.Core.Base;
using Roxana.Application.Core.Enums.Membership;
using Roxana.Application.Core.Models.Membership;

namespace Roxana.Application.Core.Contracts;

public interface IAccountService
{
    Task<OperationResult<AccountSigninResponseDto>> Sigin(AccountSigninRequestDto model);
    Task<OperationResult<AccountProfileDto>> Profile(Guid userId);
    Task<ClaimsPrincipal?> ValidateToken(string token, UserType role);
    Task<OperationResult<bool>> DeleteToken(Guid identityUserId, string token);
}