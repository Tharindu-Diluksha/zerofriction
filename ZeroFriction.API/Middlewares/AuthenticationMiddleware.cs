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

        public async Task InvokeAsync(HttpContext context)
        {
            var apiKey = context.Request.Headers["X-API-Key"];
            if (!string.IsNullOrWhiteSpace(apiKey) || apiKey != _applicationConfigurationInfo.ApiKey)
            {
                throw new UnauthorizedAccessException();
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
