using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Application.Common.Interfaces;
using CountriesApi.Domain.Configuration;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CountriesApi.Application.Common.Behaviors
{
    public class CachingBehavior<TRequest, TResponse>(
        IDatabase redis,
        IOptions<RedisSettings> redisSettings
        ) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheableQuery
    {
        private readonly RedisSettings _redisSettings = redisSettings.Value;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var cacheKey = request.CacheKey;

            var cachedData = await redis.StringGetAsync( cacheKey );
            if (!cachedData.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<TResponse>(cachedData);
            }

            var response = await next();

            // only cache non empty results
            if (response is IEnumerable<object> enumerable && enumerable.Any())
            {
                var expiry = TimeSpan.FromMinutes(_redisSettings.CacheDurationMinutes);
                await redis.StringSetAsync(cacheKey, JsonConvert.SerializeObject(response), expiry);
            }

            return response;
        }
    }
}
