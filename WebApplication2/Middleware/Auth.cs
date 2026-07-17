using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

public class CustomAuthGuardHandler : AuthorizationHandler<CustomAuthGuardRequirement>
{
    // Make sure this matches the exact secret key used to sign your tokens
    private readonly string _jwtSecret = "YourSuperSecretKeyMustBeAtLeast32BytesLong!";

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        CustomAuthGuardRequirement requirement)
    {
        // 1. In ASP.NET Core endpoint routing, context.Resource can be DefaultHttpContext or HttpContext
        if (context.Resource is HttpContext httpContext)
        {
            // 2. Extract the Authorization header
            if (httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                string headerValue = authHeader.ToString();

                // 3. Check for the "Bearer " prefix
                if (headerValue.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase))
                {
                    string token = headerValue.Substring("Bearer ".Length).Trim();

                    // 4. Validate the token
                    var tokenHandler = new JsonWebTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                        ValidateIssuer = false,    // Set to true if you set Issuer in JwtService
                        ValidateAudience = false,  // Set to true if you set Audience in JwtService
                        ValidateLifetime = true    // Ensures token hasn't expired
                    };

                    // Validate Token Signature & Expiration
                    var result = tokenHandler.ValidateToken(token, validationParameters);

                    if (result.IsValid)
                    {
                        // 5. Token is valid! Mark the authorization requirement as succeeded
                        context.Succeed(requirement);
                    }
                }
            }
        }

        return Task.CompletedTask;
    }
}