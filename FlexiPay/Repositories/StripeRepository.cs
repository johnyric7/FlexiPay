using Stripe;
using System.Threading.Tasks;

namespace FlexiPay.Repositories;

public class StripeRepository : IStripeRepository
{
    private readonly string _secretKey;

    public StripeRepository(string secretKey)
    {
        _secretKey = secretKey;
        StripeConfiguration.ApiKey = _secretKey;
    }

    public async Task<PaymentIntent> CreatePaymentIntentAsync(long amount, string currency)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = currency,
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);
        return paymentIntent;
    }

    public async Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentIntentId, string paymentMethodId)
    {
        var options = new PaymentIntentConfirmOptions
        {
            PaymentMethod = paymentMethodId,
        };

        var service = new PaymentIntentService();

        var paymentIntent = await service.ConfirmAsync(paymentIntentId, options);

        return paymentIntent;
    }
}