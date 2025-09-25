using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using System.Net;

namespace MyRecipeBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MyRecipeBookException)
            HandleMyRecipeBookException(context);
        else
            ThrowUnknowException(context);        
    }

    private static void HandleMyRecipeBookException(ExceptionContext context)
    {
        if (context.Exception is ErrorOnValidationException exception)
        {
            context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMessages));
            context.ExceptionHandled = true;
        }
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessageException.UNKNOW_ERROR));
    }
}
