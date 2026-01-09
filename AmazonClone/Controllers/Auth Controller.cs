using Bl.Contracts;
using Bl.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace AmazonClone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("User authentication: register and login")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ================= REGISTER =================
        [HttpPost("register")]
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Creates a new user account and returns a JWT token"
        )]
        [SwaggerResponse(200, "User registered successfully", typeof(AuthResponseDto))]
        [SwaggerResponse(400, "Email already exists or invalid input")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ================= LOGIN =================
        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Login an existing user",
            Description = "Authenticates user and returns a JWT token"
        )]
        [SwaggerResponse(200, "User logged in successfully", typeof(AuthResponseDto))]
        [SwaggerResponse(400, "Invalid credentials")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
