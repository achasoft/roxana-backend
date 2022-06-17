using Microsoft.AspNetCore.Mvc.Filters;

namespace Roxana.Endpoints.Api.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // if (!context.ModelState.IsValid)
            // {
            //     var op = OperationResult<object>.Validation();
            //     context.Result = new JsonResult(op);
            // }
        }
    }
}