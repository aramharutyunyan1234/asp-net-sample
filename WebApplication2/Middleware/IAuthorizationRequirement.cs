using Microsoft.AspNetCore.Authorization;

public class CustomAuthGuardRequirement : IAuthorizationRequirement
{
    // You can pass configuration data here if needed (e.g., required roles or permissions)
    public string RequiredPermission { get; }

    public CustomAuthGuardRequirement(string permission)
    {
        RequiredPermission = permission;
    }
}