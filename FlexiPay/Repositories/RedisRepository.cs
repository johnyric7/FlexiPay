using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Stripe;

namespace FlexiPay.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _redisDatabase;
        private static readonly TimeSpan _defaultTtl = TimeSpan.FromHours(24);

        public RedisRepository(IConnectionMultiplexer redisConnection)
        {
            _redisDatabase = redisConnection.GetDatabase(); // Get Redis database instance
        }

        public async Task<bool> ExistsAsync(string key)
        {
            // Check if the idempotency key exists in Redis
            return await _redisDatabase.KeyExistsAsync(key);
        }

        public async Task<PaymentIntent> GetPaymentIntentAsync(string key)
        {
            // Get the PaymentIntent data from Redis
            var paymentIntentData = await _redisDatabase.StringGetAsync(key);
            if (!paymentIntentData.HasValue)
            {
                return null; // If not found, return null
            }

            // Deserialize the PaymentIntent data
            return JsonConvert.DeserializeObject<PaymentIntent>(paymentIntentData);
        }

        public async Task SetPaymentIntentAsync(string key, PaymentIntent paymentIntent, TimeSpan? ttl = null)
        {
            var effectiveTtl = ttl ?? TimeSpan.FromHours(24);
            // Serialize the PaymentIntent object
            var paymentIntentData = JsonConvert.SerializeObject(paymentIntent);
            // Store the PaymentIntent in Redis with a TTL
            await _redisDatabase.StringSetAsync(key, paymentIntentData, ttl);
        }
    }
}
