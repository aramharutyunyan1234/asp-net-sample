// Services/IUserService.cs
using MyApi.Models;

namespace MyApi.Services;

public interface IUserService
{
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<LoginRequestDto?> GetUserByEmailAsync(string email);
}