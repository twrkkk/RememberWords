using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetSchool.Common.Exceptions;
using NetSchool.Services.Logger;

namespace NetSchool.Services.Filters;

public class CustomExceptionFilter : IExceptionFilter
{
    private readonly IAppLogger _logger;

    public CustomExceptionFilter(IAppLogger logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is EntityNotFoundException || context.Exception is ProcessException)
        {
            _logger.Information(context.Exception, context.Exception.Message);

            context.Result = new NotFoundObjectResult(context.Exception.Message)
            {
                StatusCode = 400,
            };
        }
    }
}
