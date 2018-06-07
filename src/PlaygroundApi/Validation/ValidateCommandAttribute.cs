using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlaygroundApi.Validation
{
    /// <summary>
    /// Validates model state before executing the method.
    /// </summary>
    public class ValidateCommandAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext"> The action context. </param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var controller = actionContext.Controller as ControllerBase;
                if (controller != null)
                {
                    actionContext.Result = controller.BadRequest(actionContext.ModelState);
                    return;
                }
            }
            base.OnActionExecuting(actionContext);
        }
    }
}
