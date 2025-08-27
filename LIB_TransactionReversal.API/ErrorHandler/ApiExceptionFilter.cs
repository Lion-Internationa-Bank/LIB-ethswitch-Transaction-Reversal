using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CUSTOR.API.ExceptionFilter
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilter> _Logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _Logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            ApiError apiError = null;
            if (context.Exception is ApiException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(ex.Message);
                apiError.errors = ex.Errors;

                context.HttpContext.Response.StatusCode = ex.StatusCode;

                _Logger.LogWarning($"Application thrown error: {ex.Message}", ex);
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;
                _Logger.LogWarning("Unauthorized Access in Controller Filter.");
            }
            else
            {
                // Unhandled errors
#if !DEBUG
                var msg = "An unhandled error occurred.";
                string stack = null;
#else
                var msg = context.Exception.GetBaseException().Message;
                var stack = context.Exception.StackTrace;
#endif

                apiError = new ApiError(msg);
                apiError.detail = stack;

                context.HttpContext.Response.StatusCode = 500;
                _Logger.LogError(new EventId(0), context.Exception, msg);
            }

            // JSON result
            context.Result = new JsonResult(apiError);
            int i= context.ModelState.Count;
            base.OnException(context);
        }
    }
}