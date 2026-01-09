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
    [SwaggerTag("Manage categories for products")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ================= GET ALL =================
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all categories",
            Description = "Retrieves a list of all product categories"
        )]
        [SwaggerResponse(200, "Returns the list of categories", typeof(List<CategoryDto>))]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // ================= GET BY ID =================
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get category by ID",
            Description = "Retrieves a single category by its ID"
        )]
        [SwaggerResponse(200, "Returns the category", typeof(CategoryDto))]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found" });

            return Ok(category);
        }

        // ================= CREATE =================
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new category",
            Description = "Adds a new category to the database"
        )]
        [SwaggerResponse(201, "Category created successfully")]
        [SwaggerResponse(400, "Invalid category data")]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _categoryService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // ================= UPDATE =================
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Update an existing category",
            Description = "Updates the details of an existing category"
        )]
        [SwaggerResponse(204, "Category updated successfully")]
        [SwaggerResponse(400, "ID mismatch or invalid data")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> Update(int id, CategoryDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            var updated = await _categoryService.UpdateAsync(dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // ================= DELETE =================
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Delete a category",
            Description = "Deletes a category by its ID"
        )]
        [SwaggerResponse(204, "Category deleted successfully")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
