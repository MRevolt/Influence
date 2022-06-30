using Cosmetic.Model.Dto.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Influence.Service.Configuration
{
    public class AdminAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new BusinessException(context.ModelState.Select(y => y.Value.Errors).Where(y => y.Any())
                    ?.FirstOrDefault()?.FirstOrDefault().ErrorMessage);
            }
        }
    }
}