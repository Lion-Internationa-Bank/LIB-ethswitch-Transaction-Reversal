using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CUSTOR.API.ExceptionFilter
{
  /// <summary>
  ///     An API Error response returned to the client
  /// </summary>
  public class ApiError
    {
        public ApiError(string message)
        {
            this.message = message;
            isError = true;
        }

        public ApiError(ModelStateDictionary modelState)
        {
            isError = true;
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
                //message = "Please correct the specified errors and try again.";

             message = string.Join(" | ", modelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage));

            string x = message;
        }

        public string message { get; set; }
        public bool isError { get; set; }
        public string detail { get; set; }
        public object data { get; set; }

        public ValidationErrorCollection errors { get; set; }
    }
}