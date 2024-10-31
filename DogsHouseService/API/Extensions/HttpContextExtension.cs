using API.Middleware.Model;

namespace API.Extensions
{
    public static class HttpContextExtension
    {
        public static bool HasRateLimitAttribute(this HttpContext context, out RateLimitAttribute? rateLimitAttribute)
        {
            rateLimitAttribute = context.GetEndpoint()?.Metadata.GetMetadata<RateLimitAttribute>();
            return rateLimitAttribute is not null;
        }
        public static string GetCustomerKey(this HttpContext context)
             => $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";
    }
}
