using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ZeroFriction.Domain;

namespace ZeroFriction.API.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationConfigurationInfo _applicationConfigurationInfo;

        public AuthenticationMiddleware(RequestDelegate next, ApplicationConfigurationInfo applicationConfigurationInfo)
        {
            _next = next;
            _applicationConfigurationInfo = applicationConfigurationInfo;
        }

        /// <summary>
        /// Check the requests for API key inclution in header and do the authentication
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Temporary disabled due to a review comment
            //Do the API Key authentication
            //var apiKey = context.Request.Headers["X-API-Key"].FirstOrDefault();
            //if ((!context.Request.Path.ToString().Contains("swagger")) && (string.IsNullOrWhiteSpace(apiKey) || !string.Equals(apiKey, _applicationConfigurationInfo.ApiKey)))
            //{
            //    throw new UnauthorizedAccessException();
            //}

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
