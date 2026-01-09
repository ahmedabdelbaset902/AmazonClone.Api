using Bl.Contracts;
using Bl.DTOs;
using DAL.Contracts;
using Domains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging; // <-- for ILogger
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger; // <-- logger injected

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto)
    {
        _logger.LogInformation($"Attempting to register user with email {dto.Email}.");

        var existing = (await _unitOfWork.Repository<User>().GetAllAsync())
            .FirstOrDefault(u => u.Email == dto.Email);

        if (existing != null)
        {
            _logger.LogWarning($"Registration failed: Email {dto.Email} already exists.");
            throw new Exception("Email already exists");
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        _logger.LogInformation($"User registered successfully with ID {user.Id}.");

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
    {
        _logger.LogInformation($"User attempting login with email {dto.Email}.");

        var user = (await _unitOfWork.Repository<User>().GetAllAsync())
            .FirstOrDefault(u => u.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning($"Login failed for email {dto.Email}: Invalid credentials.");
            throw new Exception("Invalid credentials");
        }

        var token = GenerateJwtToken(user);

        _logger.LogInformation($"User {user.Id} logged in successfully.");

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public int GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return int.Parse(jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
    }
}
