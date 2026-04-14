using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EnglishAI.Infrastructure.Services.Auth;

public sealed class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(
        Guid userId,
        string email,
        string displayName,
        string role,
        string? level,
        DateTime utcNow)
    {
        var secret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Missing Jwt:Secret.");
        var issuer = _configuration["Jwt:Issuer"] ?? "EnglishAI";
        var audience = _configuration["Jwt:Audience"] ?? "EnglishAI";

        var expMinutes = int.TryParse(_configuration["Jwt:AccessTokenExpirationMinutes"], out var m) ? m : 15;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Name, displayName),
            new(ClaimTypes.Role, role),
        };

        if (!string.IsNullOrWhiteSpace(level))
        {
            claims.Add(new Claim("level", level));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: utcNow,
            expires: utcNow.AddMinutes(expMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token, bool validateLifetime)
    {
        var secret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Missing Jwt:Secret.");
        var issuer = _configuration["Jwt:Issuer"] ?? "EnglishAI";
        var audience = _configuration["Jwt:Audience"] ?? "EnglishAI";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.FromSeconds(30),
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.ValidateToken(token, parameters, out _);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}

