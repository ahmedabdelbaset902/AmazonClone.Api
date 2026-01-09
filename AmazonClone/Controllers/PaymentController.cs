using Microsoft.AspNetCore.Mvc;
using Bl.Contracts;
using Bl.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace AmazonClone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Handle payments and payment intents")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // ================= CREATE PAYMENT INTENT =================
        [HttpPost("create-payment-intent")]
        [SwaggerOperation(
            Summary = "Create payment intent",
            Description = "Creates a payment intent and returns a client secret to be used on the frontend for completing the payment"
        )]
        [SwaggerResponse(200, "Payment intent created successfully", typeof(object))]
        [SwaggerResponse(400, "Invalid payment data")]
        public async Task<IActionResult> CreatePaymentIntent(
            [FromBody] CreatePaymentIntentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clientSecret = await _paymentService
                .CreatePaymentIntentAsync(dto.Amount, dto.Currency, dto.OrderId);

            return Ok(new
            {
                clientSecret
            });
        }
    }
}
