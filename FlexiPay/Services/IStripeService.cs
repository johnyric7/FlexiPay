using Stripe;
using System.Threading.Tasks;

namespace FlexiPay.Services;

public interface IStripeService
{
    Task<PaymentIntent> CreatePaymentAsync(long amount, string currency, string idempotencyKey);
    Task<PaymentIntent> ConfirmPaymentAsync(string paymentIntentId, string paymentMethodId);
}
