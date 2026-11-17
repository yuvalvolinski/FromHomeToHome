using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using FromHomeToHome.Core.DTOs;

namespace FromHomeToHome.Backend.Services;

public interface IUserService
{
    Task<bool> UserExistsAsync(string username, string email);
    Task<int> RegisterUserAsync(RegisterDto registerDto);
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
}

public class UserService : IUserService
{
    private readonly string _connectionString;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IConfiguration configuration,
        ILogger<UserService> logger,
        IPasswordHasher passwordHasher)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> UserExistsAsync(string username, string email)
    {
        try
        {
            _logger.LogInformation("Checking DB for existing user: Username={Username}, Email={Email}", username, email);

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            string query = @"
                SELECT COUNT(1) 
                FROM Users 
                WHERE LOWER(Username) = LOWER(@Username) OR LOWER(Email) = LOWER(@Email)";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Email", email);

            var count = Convert.ToInt32(await command.ExecuteScalarAsync());
            return count > 0;
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error during UserExists check for Username={Username}", username);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in UserExistsAsync for Username={Username}", username);
            throw;
        }
    }

    public async Task<int> RegisterUserAsync(RegisterDto registerDto)
    {
        try
        {
            _logger.LogInformation("Hashing password and inserting new user: Username={Username}", registerDto.Username);

            string hashedPassword = _passwordHasher.HashPassword(registerDto.Password);

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            string query = @"
                INSERT INTO Users (Username, Email, PasswordHash, CreatedAt, CurrentLevel, TotalScore)
                VALUES (@Username, @Email, @PasswordHash, @CreatedAt, @CurrentLevel, @TotalScore);
                SELECT last_insert_rowid();";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Username", registerDto.Username);
            command.Parameters.AddWithValue("@Email", registerDto.Email);
            command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
            command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow.ToString("o"));
            command.Parameters.AddWithValue("@CurrentLevel", 1);
            command.Parameters.AddWithValue("@TotalScore", 0);

            var newId = Convert.ToInt32(await command.ExecuteScalarAsync());

            _logger.LogInformation("User {Username} registered successfully with ID={UserId}", registerDto.Username, newId);
            return newId;
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error during registration for Username={Username}", registerDto.Username);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration for Username={Username}", registerDto.Username);
            throw;
        }
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            _logger.LogInformation("Checking credentials in DB for Email={Email}", loginDto.Email);

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            string query = @"
                SELECT Id, Username, Email, PasswordHash
                FROM Users
                WHERE LOWER(Email) = LOWER(@Email)
                LIMIT 1";

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Email", loginDto.Email.Trim());

            using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                _logger.LogWarning("Login failed: Email '{Email}' not found.", loginDto.Email);
                return null;
            }

            int userId = reader.GetInt32(reader.GetOrdinal("Id"));
            string username = reader.GetString(reader.GetOrdinal("Username"));
            string email = reader.GetString(reader.GetOrdinal("Email"));
            string passwordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));

            bool isValid = _passwordHasher.VerifyPassword(loginDto.Password, passwordHash);
            if (!isValid)
            {
                _logger.LogWarning("Login failed: Invalid password for UserId={UserId}", userId);
                return null;
            }

            _logger.LogInformation("User {Username} (ID={UserId}) authenticated successfully.", username, userId);

            return new LoginResponseDto
            {
                UserId = userId,
                Username = username,
                Email = email
            };
        }
        catch (SqliteException ex)
        {
            _logger.LogError(ex, "Database error during login for Email={Email}", loginDto.Email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for Email={Email}", loginDto.Email);
            throw;
        }
    }
}
