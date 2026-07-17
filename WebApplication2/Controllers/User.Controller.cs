// Controllers/UserController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.Services;

namespace MyApi.Controllers;

[ApiController]
[Route("user")] // matches GET /user
[Authorize(Policy = "MyCustomGuard")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    // service is injected via constructor, same DI as before
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")] // 1. Defines the route template (e.g., api/users/5)
    public async Task<IActionResult> GetById(int id) // 2. Binds the URL parameter to this variable
    {
        // 3. Pass the id to your service to fetch a single user
        var user = await _userService.GetUserByIdAsync(id);
    
        if (user == null)
        {
            return NotFound($"User with ID {id} not found."); // Return 404 if not found
        }

        return Ok(user);
    }
    
    // // Controllers/UserController.cs
    // [HttpPost]
    // public async Task<IActionResult> CreateUser([FromBody] User user)
    // {
    //     if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
    //     {
    //         return BadRequest("Name and Email are required.");
    //     }
    //
    //     var createdUser = await _userService.CreateUserAsync(user);
    //
    //     // 201 Created, with a Location header pointing to GET /user (or /user/{id} if you have one)
    //     return CreatedAtAction(nameof(GetUsers), new { id = createdUser.Id }, createdUser);
    // }
}