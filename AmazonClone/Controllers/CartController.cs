using Bl.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace AmazonClone.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [SwaggerTag("Manage user's cart and its items")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // ================= GET USER CART =================
        [HttpGet("{userId}")]
        [SwaggerOperation(
            Summary = "Get user's cart",
            Description = "Retrieves the cart details for a specific user"
        )]
        [SwaggerResponse(200, "Returns the user's cart")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }

        // ================= ADD PRODUCT TO CART =================
        [HttpPost("{userId}/add")]
        [SwaggerOperation(
            Summary = "Add product to cart",
            Description = "Adds a product with a specified quantity to the user's cart"
        )]
        [SwaggerResponse(200, "Product added successfully")]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> AddProduct(int userId, int productId, int quantity)
        {
            await _cartService.AddProductAsync(userId, productId, quantity);
            return Ok();
        }

        // ================= UPDATE PRODUCT QUANTITY =================
        [HttpPut("{userId}/update")]
        [SwaggerOperation(
            Summary = "Update product quantity in cart",
            Description = "Updates the quantity of a specific product in the user's cart"
        )]
        [SwaggerResponse(204, "Product quantity updated successfully")]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> UpdateQuantity(int userId, int productId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(userId, productId, quantity);
            return NoContent();
        }

        // ================= REMOVE PRODUCT FROM CART =================
        [HttpDelete("{userId}/remove")]
        [SwaggerOperation(
            Summary = "Remove product from cart",
            Description = "Removes a specific product from the user's cart"
        )]
        [SwaggerResponse(204, "Product removed successfully")]
        [SwaggerResponse(404, "Product not found in cart")]
        public async Task<IActionResult> RemoveProduct(int userId, int productId)
        {
            await _cartService.RemoveProductAsync(userId, productId);
            return NoContent();
        }
    }
}
