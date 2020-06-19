using CloudTrader.Traders.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CloudTrader.Traders.Api.Exceptions
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case TraderAlreadyExistsException exception:
                    context.Result = new ConflictObjectResult(exception.Message);
                    break;
                case TraderNotFoundException exception:
                    context.Result = new NotFoundObjectResult(exception.Message);
                    break;
                default:
                    context.Result = new StatusCodeResult(500);
                    break;
            }
        }
    }
}
