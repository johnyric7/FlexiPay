namespace FlexiPay.Models.Stripe;

public class CreatePaymentRequest
{
    public long Amount { get; set; } // Amount in cents
    public string Currency { get; set; }
    public string IdempotencyKey { get; set; }
}