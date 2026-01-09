using Bl.Contracts;
using Bl.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace AmazonClone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Manage orders and checkout from user carts")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ================= CHECKOUT =================
        [HttpPost("checkout/{userId:int}")]
        [SwaggerOperation(
            Summary = "Checkout user's cart",
            Description = "Creates an order from the items in the user's cart and clears the cart"
        )]
        [SwaggerResponse(200, "Order created successfully", typeof(object))]
        [SwaggerResponse(400, "Cart is empty or invalid user ID")]
        public async Task<IActionResult> Checkout(int userId)
        {
            try
            {
                var orderId = await _orderService.CheckoutAsync(userId);
                return Ok(new { orderId });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ================= GET ORDERS FOR USER =================
        [HttpGet("{userId:int}")]
        [SwaggerOperation(
            Summary = "Get all orders for a user",
            Description = "Fetches all orders placed by the user with the given ID"
        )]
        [SwaggerResponse(200, "Returns the list of orders", typeof(System.Collections.Generic.List<OrderDto>))]
        [SwaggerResponse(404, "User has no orders")]
        public async Task<IActionResult> GetOrdersForUser(int userId)
        {
            var orders = await _orderService.GetOrdersForUserAsync(userId);
            if (orders == null || orders.Count == 0)
                return NotFound(new { message = $"No orders found for user with ID {userId}" });

            return Ok(orders);
        }
    }
}
