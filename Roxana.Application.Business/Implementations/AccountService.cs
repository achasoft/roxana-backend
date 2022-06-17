using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Roxana.Application.Core.Base;
using Roxana.Application.Core.Contracts;
using Roxana.Application.Core.Enums.Membership;
using Roxana.Application.Core.Models.Membership;

namespace Roxana.Application.Business.Implementations;

internal class AccountService : IAccountService
{
    private readonly IServiceProvider _serviceProvider;

    public AccountService( IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public ClaimsPrincipal ExtractToken(string token, UserType role)
    {
        throw new NotImplementedException();
    }

    public async Task<OperationResult<AccountSigninResponseDto>> Sigin(AccountSigninRequestDto model)
    {
        try
        {
            await Task.Delay(500);
            return OperationResult<AccountSigninResponseDto>.Success(new AccountSigninResponseDto
            {
                Token = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid()
            });
        }
        catch (Exception ex)
        {
            _serviceProvider.GetService<ILogger>()!.LogError(ex, "Sigin failed for : {0}", model.Email);
            return OperationResult<AccountSigninResponseDto>.Fail();
        }
    }

    public Task<OperationResult<AccountProfileDto>> Profile(Guid userId)
    {
        return Task.FromResult(OperationResult<AccountProfileDto>.Success(new AccountProfileDto
        {
            Timezone = "Tehran/Iran"
        }));
    }

    public Task<ClaimsPrincipal?> ValidateToken(string token, UserType role)
    {
        var identities = new List<ClaimsIdentity>
        {
            new ClaimsIdentity(
                claims: new List<Claim>
                {
                    new (RoxanaClaims.AuthUserId, Guid.NewGuid().ToString()),
                    new (RoxanaClaims.AuthUserName, "Navid Kianfar"),
                    new (RoxanaClaims.AuthUserType, role.ToString())
                },
                authenticationType: "roxana",
                nameType: "roxana",
                roleType: "roxana"
            )
        };
        var result = new ClaimsPrincipal(identities);
        return Task.FromResult(result);
    }
}