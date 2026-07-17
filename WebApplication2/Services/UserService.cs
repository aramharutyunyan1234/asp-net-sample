// Services/UserService.cs

using System.Runtime.InteropServices.JavaScript;
using Npgsql;
using MyApi.Models;

namespace MyApi.Services;

public class UserService : IUserService
{
    private readonly string _connectionString;

    public UserService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string not found");
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = new List<User>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand("SELECT * FROM users", connection);
        await using var reader = await command.ExecuteReaderAsync();
        

        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Email = reader.GetString(reader.GetOrdinal("email"))
            });
        }
        return users;
    }

    public async Task<User?> GetUserByIdAsync(int id) // 1. Accept id parameter (and return a single User, or null)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        // 2. Use a placeholder (@id) in the SQL string
        string sql = "SELECT id, name, email FROM users WHERE id = @id";
    
        await using var command = new NpgsqlCommand(sql, connection);
    
        // 3. Safely map the parameter value to the placeholder
        command.Parameters.AddWithValue("id", id);

        await using var reader = await command.ExecuteReaderAsync();

        // 4. Since ID is unique, use 'if' instead of 'while' to get a single record
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Email = reader.GetString(reader.GetOrdinal("email"))
            };
        }

        return null; // Return null if no user matched that ID
    }
    
    public async Task<LoginRequestDto?> GetUserByEmailAsync(string email)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        // 2. Use a placeholder (@id) in the SQL string
        string sql = "SELECT id, email, password FROM users WHERE email = @email";
    
        await using var command = new NpgsqlCommand(sql, connection);
    
        // 3. Safely map the parameter value to the placeholder
        command.Parameters.AddWithValue("email", email);

        await using var reader = await command.ExecuteReaderAsync();

        // 4. Since ID is unique, use 'if' instead of 'while' to get a single record
        if (await reader.ReadAsync())
        {
            return new LoginRequestDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                Password = reader.GetString(reader.GetOrdinal("password"))
            };
        }

        return null; // Return null if no user matched that ID
    }
}