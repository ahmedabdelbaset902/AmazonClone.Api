using Stripe;
using Bl.Contracts;
using Microsoft.Extensions.Configuration;

public class PaymentService : IPaymentService
{
    public PaymentService(IConfiguration configuration)
    {
        StripeConfiguration.ApiKey =
            configuration["Stripe:SecretKey"]
            ?? throw new Exception("Stripe Secret Key not found");
    }

    public async Task<string> CreatePaymentIntentAsync(
     decimal amount,
     string currency = "usd",
     int orderId = 0)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = currency,
            Metadata = new Dictionary<string, string>
        {
            { "order_id", orderId.ToString() }
        }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        return paymentIntent.ClientSecret;
    }

}

