using Microsoft.Data.Sqlite;

namespace FromHomeToHome.Backend.Data;

public class DbInitializer
{
    private readonly string _connectionString;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(IConfiguration configuration, ILogger<DbInitializer> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _logger = logger;
    }

    public async Task InitializeDatabaseAsync()
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            // Create Users table
            string usersTableQuery = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    CurrentLevel INTEGER NOT NULL DEFAULT 1,
                    TotalScore INTEGER NOT NULL DEFAULT 0,
                    CreatedAt TEXT NOT NULL,
                    CONSTRAINT CheckCurrentLevel CHECK (CurrentLevel >= 1)
                );";

            using var command = connection.CreateCommand();
            command.CommandText = usersTableQuery;
            await command.ExecuteNonQueryAsync();

            _logger.LogInformation("Database initialized successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing database");
            throw;
        }
    }
}
