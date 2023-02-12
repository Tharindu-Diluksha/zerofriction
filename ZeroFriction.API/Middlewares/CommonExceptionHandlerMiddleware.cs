using System.Net;
using ZeroFriction.DB.Domain.Exceptions;
using ZeroFriction.Domain.Exceptions;

namespace ZeroFriction.API.Middlewares
{
    public class CommonExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public CommonExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                // Call the next delegate/middleware in the pipeline.
                await _next(context);
            }
            catch (UnauthorizedAccessException)
            {
                await HandleException(context, string.Empty, HttpStatusCode.Unauthorized);
            }
            catch (DocumentNotFoundException ex)
            {
                await HandleException(context, string.Empty, HttpStatusCode.NotFound);
            }
            catch (ConcurrencyException ex)
            {
                await HandleException(context, string.Empty, HttpStatusCode.Conflict);
            }
            catch (BusinessException ex)
            {
                await HandleException(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
#if DEBUG
                await HandleException(context, ex.Message, HttpStatusCode.BadRequest);
#else
                await HandleException(context, "ERROR", HttpStatusCode.BadRequest);
#endif
            }

        }

        /// <summary>
        /// Set status code and error message for the response
        /// </summary>
        /// <param name="context"></param>
        /// <param name="errorMessage"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private async Task HandleException(HttpContext context, string errorMessage, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(errorMessage);
        }
    }
}
