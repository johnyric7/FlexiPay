using Microsoft.AspNetCore.Mvc;
using FlexiPay.Services;
using Stripe;
using System;
using System.Threading.Tasks;
using FlexiPay.Models.Stripe;

namespace FlexiPay.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StripeController : ControllerBase
{
    private readonly IStripeService _stripeService;

    public StripeController(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest("Amount should be greater than zero.");
        }

        try
        {
            var paymentIntent = await _stripeService.CreatePaymentAsync(request.Amount, request.Currency, request.IdempotencyKey);

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
        catch (StripeException stripeEx)
        {
            return StatusCode(500, new { message = "Stripe Error: " + stripeEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error: " + ex.Message });
        }
    }

    [HttpPost("confirm-payment-intent")]
    public async Task<IActionResult> ConfirmPaymentIntent([FromBody] ConfirmPaymentRequest request)
    {
        var paymentIntent = await _stripeService.ConfirmPaymentAsync(request.PaymentIntentId, request.PaymentMethodId);

        if (paymentIntent.Status == "succeeded")
        {
            return Ok(new { status = "Payment succeeded" });
        }
        else
        {
            return BadRequest(new { status = "Payment failed" });
        }
    }
}