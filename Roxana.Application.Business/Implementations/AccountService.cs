using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Roxana.Application.Core.Base;
using Roxana.Application.Core.Contracts;
using Roxana.Application.Core.Enums.Membership;
using Roxana.Application.Core.Helpers;
using Roxana.Application.Core.Models.Membership;
using Roxana.Application.Data.Contextes;
using Roxana.Application.Data.Models;

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
            using (var unit = _serviceProvider.GetService<AccountDbContext>())
            {
                // TODO: find the user here...
                
                // var user2 = new User
                // {
                //     Type = UserType.Roxana,
                //     Username = "nvd.kianfar",
                //     Email = "nvd.kianfar@gmail.com",
                //     Phone = "905392282379",
                //     FullName = "Navid Kianfar",
                //     Password = "ach@$0ft1361986",
                //     Salt = CryptoHelper.CreateSalt(64),
                //     Timezone = "Europe/Istanbul"
                // };
                // user2.Password = CryptoHelper.EncryptPassword(user2.Password, user2.Salt);
                // await unit.Users.AddAsync(user2);
                // await unit.SaveChangesAsync();


                var user = await unit.Users
                    .AsNoTracking()
                    .OrderByDescending(i => i.CreatedAt)
                    .FirstOrDefaultAsync(i => i.Email == model.Email && !i.DeletedAt.HasValue);
                
                if (user == null || user.DeletedAt.HasValue) 
                    return OperationResult<AccountSigninResponseDto>.NotFound();
                
                if (!CryptoHelper.VerifyPassword(model.Password, user.Password))
                    return OperationResult<AccountSigninResponseDto>.NotFound();
                    
                var jsonService = _serviceProvider.GetService<IJsonService>();
                var expiresIn = DateTime.UtcNow.AddMonths(6);
                var token = new UserToken
                {
                    UserId = user.Id,
                    Expires = expiresIn,
                    Token = jsonService.Serialize(
                        new TokenClaimsViewModel(user.Username, user.Id, user.Type, expiresIn)
                    ),
                };

                token.Token = CryptoHelper.EncryptSHA512(token.Token, user.Salt);
                token.Hash = CryptoHelper.Sha1($"{user.Id}-{token.Id}");
                await unit.Tokens.AddAsync(token);
                await unit.SaveChangesAsync();
                
                // TODO: add token to redis server...
                
                return OperationResult<AccountSigninResponseDto>.Success(new AccountSigninResponseDto
                {
                    Token = token.Hash,
                    UserId = user.Id
                });
            }
        }
        catch (Exception ex)
        {
            _serviceProvider.GetService<ILogger>()!.LogError(ex, "Sigin failed for : {0}", model.Email);
            return OperationResult<AccountSigninResponseDto>.Fail();
        }
    }

    public async Task<OperationResult<AccountProfileDto>> Profile(Guid userId)
    {
        try
        {
            using (var unit = _serviceProvider.GetService<AccountDbContext>())
            {
                var user = await unit.Users
                    .Where(i => i.Id == userId && !i.DeletedAt.HasValue)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
                
                return OperationResult<AccountProfileDto>.Success(new AccountProfileDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    UserType = user.Type,
                    Timezone = user.Timezone,
                    FullName = user.FullName,
                });
            }
        }
        catch (Exception ex)
        {
            _serviceProvider.GetService<ILogger>()!.LogError(ex, "Get profile failed for : {0}", userId);
            return OperationResult<AccountProfileDto>.Fail();
        }
    }

    public async Task<ClaimsPrincipal?> ValidateToken(string hash, UserType role)
    {
        try
        {
            using (var unit = _serviceProvider.GetService<AuthorizeDbContext>())
            {
                var token = await unit.Tokens
                    .Include(i => i.User)
                    .Where(i => i.Hash == hash && !i.DeletedAt.HasValue)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();

                if (token == null) return null;
                var jsonService = _serviceProvider.GetService<IJsonService>();

                var decrypted = CryptoHelper.DecryptSHA512(token.Token, token.User.Salt);
                var claims = jsonService.Deserialize<TokenClaimsViewModel>(decrypted);

                return new ClaimsPrincipal(new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(
                        claims: new List<Claim>
                        {
                            new (RoxanaClaims.AuthUserName, token.User.FullName),
                            new (RoxanaClaims.AuthUserId, claims.UserId.ToString()),
                            new (RoxanaClaims.AuthUserType, claims.UserType.ToString())
                        },
                        authenticationType: "roxana",
                        nameType: "roxana",
                        roleType: "roxana"
                    )
                });
            }
        }
        catch (Exception ex)
        {
            _serviceProvider.GetService<ILogger>()!.LogError(ex, "Token validation failed for : {0}", hash);
            return null;
        }
    }
}