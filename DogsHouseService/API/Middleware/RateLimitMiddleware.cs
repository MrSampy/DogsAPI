using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using API.Extensions;

namespace API.Middleware
{
    public class RateLimitMiddleware(
        RequestDelegate next,
        IDistributedCache cache)
    {
        private readonly RequestDelegate _next = next;
        private readonly IDistributedCache _cache = cache;

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.HasRateLimitAttribute(out var decorator))
            {
                await _next(context);
                return;
            }

            var consumptionData = await _cache.GetCustomerConsumptionDataFromContextAsync(context);
            if (consumptionData is not null)
            {
                if (consumptionData.HasConsumedAllRequests(decorator!.TimeWindowInSeconds, decorator!.MaxRequests))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    return;
                }

                consumptionData.IncreaseRequests(decorator!.MaxRequests);
            }

            await _cache.SetCacheValueAsync(context.GetCustomerKey(), consumptionData);

            await _next(context);
        }
    }
}
