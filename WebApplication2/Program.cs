using Microsoft.AspNetCore.Authorization;
using MyApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddControllers(); // 👈 add this — enables controller routing
builder.Services.AddHttpContextAccessor(); // Needed to access HttpContext in the handler
builder.Services.AddSingleton<IAuthorizationHandler, CustomAuthGuardHandler>();

builder.Services.AddAuthorization(options =>
{
    // Register the policy with your custom requirement
    options.AddPolicy("MyCustomGuard", policy =>
        policy.Requirements.Add(new CustomAuthGuardRequirement("ReadAccess")));
});

var app = builder.Build();





app.MapControllers(); // 👈 add this — wires up [Route]/[HttpGet] attributes

// remove or comment out the old minimal API line, now handled by UserController:
// app.MapGet("/user", async (IUserService userService) => { ... });

app.Run();