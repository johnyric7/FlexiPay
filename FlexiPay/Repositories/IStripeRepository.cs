using System.Threading.Tasks;
using Stripe;

namespace FlexiPay.Repositories;

public interface IStripeRepository
{
    Task<PaymentIntent> CreatePaymentIntentAsync(long amount, string currency);
    Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentIntentId, string paymentMethodId);
}