using System;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Roulette.Application.Exceptions;

namespace Roulette.Api.Middleware
{
    public class ExceptionHandling
    {
        public class ExceptionHandlerMiddleware
        {
            private readonly RequestDelegate _next;

            public ExceptionHandlerMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    await ConvertException(context, ex);
                }
            }

            private static Task ConvertException(HttpContext context, Exception exception)
            {
                HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";

                var result = string.Empty;

                switch (exception)
                {
                    case ValidationException validationException:
                        httpStatusCode = HttpStatusCode.BadRequest;
                        result = JsonConvert.SerializeObject(validationException.ValidationErrors);
                        break;
                    case BadRequestException badRequestException:
                        result = badRequestException.Message;
                        break;
                    case NotFoundException:
                        httpStatusCode = HttpStatusCode.NotFound;
                        break;
                    case Exception:
                        httpStatusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                context.Response.StatusCode = (int)httpStatusCode;

                if (result == string.Empty)
                {
                    result = JsonConvert.SerializeObject(new { error = exception.Message });
                }

                return context.Response.WriteAsync(result);
            }
        }
    }
}

