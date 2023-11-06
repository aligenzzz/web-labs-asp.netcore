using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Web_153505_Bybko.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try 
            { 
                await _next.Invoke(context); 
            }
            catch (Exception ex) when (ex.InnerException is OpenIdConnectProtocolException)
            {
                context.Response.Redirect("/");
            }

            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            {
                var logInformation = new
                {
                    RequestPath = context.Request.Path,
                    ResponseStatusCode = context.Response.StatusCode
                };
                string currentDate = DateTime.UtcNow.Date.ToString();

                _logger.LogInformation($"---> request {logInformation.RequestPath} returns {logInformation.ResponseStatusCode}");
            }
        }
    }
}
