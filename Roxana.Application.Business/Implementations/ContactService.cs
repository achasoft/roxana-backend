using Roxana.Application.Core.Base;
using Roxana.Application.Core.Contracts;
using Roxana.Application.Core.Models.Contact;
using Roxana.Application.Core.Models.Workspace;

namespace Roxana.Application.Business.Implementations;

internal class ContactService : IContactService
{
    public Task<OperationResult<GridResult<ContactDto>>> ListPaginated(GridFilterWithParams<SpaceParamDto> model)
    {
        var result = OperationResult<GridResult<ContactDto>>.EmptyResult<ContactDto>();
        return Task.FromResult(result);
    }
}