using Microsoft.AspNetCore.Mvc;
using Roxana.Application.Core.Base;
using Roxana.Application.Core.Contracts;
using Roxana.Application.Core.Models.Workspace;

namespace Roxana.Endpoints.Api.Controllers.Contact;

[Route("api/contacts")]
[ApiExplorerSettings(GroupName = "Contact")]
public class ContactController : BaseController
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }
    
    [HttpPost]
    [Route("list-paginated")]
    public async Task<IActionResult> ListPaginated([FromBody] GridFilterWithParams<SpaceParamDto> model)
    {
        var op = await _contactService.ListPaginated(model);
        return Json(op);
    }
}