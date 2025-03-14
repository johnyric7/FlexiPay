using FlexiPay.Repositories;
using Stripe;
using System.Threading.Tasks;

namespace FlexiPay.Services;

public class StripeService : IStripeService
{
    private readonly IRedisRepository _redisRepository;
    private readonly IStripeRepository _stripeRepository;

    public StripeService(IRedisRepository redisRepository, IStripeRepository stripeRepository)
    {
        _redisRepository = redisRepository;
        _stripeRepository = stripeRepository;
    }

    public async Task<PaymentIntent> CreatePaymentAsync(long amount, string currency, string idempotencyKey)
    {
        // Check if the idempotency key already exists in the redis
        if (await _redisRepository.ExistsAsync(idempotencyKey))
        {
            var existingPaymentIntent = await _redisRepository.GetPaymentIntentAsync(idempotencyKey);
            return new PaymentIntent() { ClientSecret = existingPaymentIntent.ClientSecret };
        }

        // If the idempotency key does not exist, create a new PaymentIntent via Stripe
        var paymentIntent = await _stripeRepository.CreatePaymentIntentAsync(amount, currency);

        // Store the new PaymentIntent in Redis with the idempotency key
        await _redisRepository.SetPaymentIntentAsync(idempotencyKey, paymentIntent);

        return paymentIntent;
    }

    public async Task<PaymentIntent> ConfirmPaymentAsync(string paymentIntentId, string paymentMethodId)
    {
        return await _stripeRepository.ConfirmPaymentIntentAsync(paymentIntentId, paymentMethodId);
    }
}
