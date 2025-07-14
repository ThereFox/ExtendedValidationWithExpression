#region

using Microsoft.AspNetCore.Mvc.Filters;

#endregion

namespace ExtendedValidation.AspNet;

public class RequestValidatorMiddlware : IActionFilter
{
    private readonly ValidateService _validateService;

    public RequestValidatorMiddlware(ValidateService validateService)
    {
        _validateService = validateService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var arguments = context.ActionArguments;

        if (arguments.Count == 0) return;

        if (arguments.Count > 1) return;

        var request = arguments.First().Value;

        Convert.ChangeType(request, arguments.First().Value.GetType());

        var validateResult = _validateService.Validate(request);

        if (validateResult.IsFailure)
        {
            context.HttpContext.Response.StatusCode = 400;
            context.HttpContext.Response.WriteAsync(validateResult.Error).Wait();
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}