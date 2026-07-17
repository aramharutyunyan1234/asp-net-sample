using Microsoft.AspNetCore.Mvc;
using MyApi.Helpers;
using MyApi.Services;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")] // Route will be: /api/login
public class LoginController : ControllerBase // <--- Changed from Controller to ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtService _jwtService;

    public LoginController(IUserService userService, JwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        // 1. Fetch user by email
        var user = await _userService.GetUserByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        // 2. Verify Argon2 password
        bool isPasswordValid = PasswordHasher.VerifyPassword(model.Password, user.Password);
        if (!isPasswordValid)
        {
            return Unauthorized("Invalid credentials1.");
        }

        // 3. Generate JWT Token
        string token = await _jwtService.GenerateToken(user.Id, user.Email, "user");

        // 4. Return response (sanitizing password hash out of response for security)
        return Ok(new 
        { 
            AccessToken = token, 
            User = new 
            { 
                user.Id, 
                user.Email 
            } 
        });
    }
}