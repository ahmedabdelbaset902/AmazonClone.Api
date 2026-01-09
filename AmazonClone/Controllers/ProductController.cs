using Bl.Contracts;
using Bl.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazonClone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Manage products in the Amazon Clone project")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ================= GET ALL =================
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all products",
            Description = "Fetches a list of all products available in the system"
        )]
        [SwaggerResponse(200, "Returns the list of products", typeof(List<ProductDto>))]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // ================= GET BY ID =================
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get product by ID",
            Description = "Fetches a single product by its unique ID"
        )]
        [SwaggerResponse(200, "Returns the product", typeof(ProductDto))]
        [SwaggerResponse(404, "Product not found")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found" });

            return Ok(product);
        }

        // ================= CREATE =================
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new product",
            Description = "Adds a new product to the system with a valid category"
        )]
        [SwaggerResponse(201, "Product created successfully", typeof(object))]
        [SwaggerResponse(400, "Invalid product data")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = await _productService.CreateProductAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = productId },
                new { id = productId }
            );
        }

        // ================= UPDATE =================
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Update a product",
            Description = "Updates an existing product by ID"
        )]
        [SwaggerResponse(204, "Product updated successfully")]
        [SwaggerResponse(400, "Invalid product data or ID mismatch")]
        [SwaggerResponse(404, "Product not found")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            var updated = await _productService.UpdateProductAsync(dto);

            if (!updated)
                return NotFound(new { message = $"Product with ID {id} not found" });

            return NoContent();
        }

        // ================= DELETE =================
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Delete a product",
            Description = "Deletes a product by its ID"
        )]
        [SwaggerResponse(204, "Product deleted successfully")]
        [SwaggerResponse(404, "Product not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Product with ID {id} not found" });

            return NoContent();
        }
    }
}
