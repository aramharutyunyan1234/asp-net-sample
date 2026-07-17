using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

public class JwtService
{
    private readonly string _jwtSecret = "YourSuperSecretKeyMustBeAtLeast32BytesLong!";

    // In C#, tasks returning strings don't need 'async' unless they do real async I/O inside.
    // Creating a token in memory is synchronous, so returning a Task<string> is clean.
    public Task<string> GenerateToken(int userId, string email, string role)
    {
        var handler = new JsonWebTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));

        // Create the payload claims (equivalent to your payload object)
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Equivalent to expiresIn: '1h'
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        // Generate and sign the JWT string
        string token = handler.CreateToken(tokenDescriptor);

        return Task.FromResult(token);
    }
}