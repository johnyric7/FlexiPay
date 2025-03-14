using StackExchange.Redis;
using FlexiPay.Models.Stripe;
using System.Threading.Tasks;
using Stripe;
using System;

namespace FlexiPay.Repositories
{
    public interface IRedisRepository
    {
        Task<bool> ExistsAsync(string key);
        Task<PaymentIntent> GetPaymentIntentAsync(string key);
        Task SetPaymentIntentAsync(string key, PaymentIntent paymentIntent, TimeSpan? ttl = null);
    }
}
