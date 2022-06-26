using Roxana.Application.Core.Base;
using Roxana.Application.Core.Models.Contact;
using Roxana.Application.Core.Models.Workspace;

namespace Roxana.Application.Core.Contracts;

public interface IContactService
{
    Task<OperationResult<GridResult<ContactDto>>> ListPaginated(GridFilterWithParams<SpaceParamDto> model);
}