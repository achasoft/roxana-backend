using Microsoft.AspNetCore.Mvc;
using Roxana.Application.Core.Contracts;
using Roxana.Application.Core.Models.Membership;
using Roxana.Endpoints.Api.Filters;

namespace Roxana.Endpoints.Api.Controllers.Membership;

[Route("api/account")]
[ApiExplorerSettings(GroupName = "Membership")]
public class AccountController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> Signin([FromBody] AccountSigninRequestDto model)
    {
        var op = await _accountService.Sigin(model);
        return Json(op);
    }

    [HttpDelete]
    [Route("token")]
    [RoxanaAuthorize]
    public async Task<IActionResult> DeleteToken()
    {
        var token = Request.Headers["Authorization"].ToString();
        var op = await _accountService.DeleteToken(Identity.UserId, token);
        return Json(op);
    }

    [HttpGet]
    [Route("profile")]
    [RoxanaAuthorize]
    public async Task<IActionResult> Profile()
    {
        var op = await _accountService.Profile(Identity.UserId);
        return Json(op);
    }
}