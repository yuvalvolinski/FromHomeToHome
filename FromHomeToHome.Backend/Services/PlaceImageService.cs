using Microsoft.Data.Sqlite;
using FromHomeToHome.Core.DTOs;

namespace FromHomeToHome.Backend.Services;

public interface IPlaceImageService
{
    Task<RandomPlaceImageDto> GetRandomImageAsync();
    Task<string> GetImagePathAsync(int locationId);
}

public class PlaceImageService : IPlaceImageService
{
    private readonly string _connectionString;
    private readonly string _placesDirectory;
    private readonly ILogger<PlaceImageService> _logger;

    public PlaceImageService(IConfiguration configuration, IWebHostEnvironment environment, ILogger<PlaceImageService> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _placesDirectory = Path.Combine(environment.ContentRootPath, "Places");
        _logger = logger;
    }

    public async Task<RandomPlaceImageDto> GetRandomImageAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = "SELECT Id FROM Locations ORDER BY RANDOM() LIMIT 1;";
        using var command = new SqliteCommand(sql, connection);

        var result = await command.ExecuteScalarAsync();
        if (result == null || result == DBNull.Value)
        {
            _logger.LogWarning("לא נמצאו מיקומים בטבלת Locations.");
            throw new InvalidOperationException("לא נמצאו מיקומים זמינים.");
        }

        var locationId = Convert.ToInt32(result);

        return new RandomPlaceImageDto
        {
            LocationId = locationId,
            ImageUrl = $"/api/place-images/file/{locationId}",
            Description = "תמונה אקראית מוכנה למסך המשחק."
        };
    }

    public async Task<string> GetImagePathAsync(int locationId)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = "SELECT FileName FROM Locations WHERE Id = @Id LIMIT 1;";
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", locationId);

        var fileName = (await command.ExecuteScalarAsync())?.ToString();
        if (string.IsNullOrWhiteSpace(fileName))
        {
            _logger.LogWarning("לא נמצא מיקום עם מזהה {LocationId}.", locationId);
            throw new FileNotFoundException("המיקום המבוקש לא נמצא.");
        }

        var fullPath = Path.Combine(_placesDirectory, fileName);
        if (!File.Exists(fullPath))
        {
            _logger.LogWarning("קובץ התמונה לא נמצא בדיסק: {ImagePath}", fullPath);
            throw new FileNotFoundException("קובץ התמונה המבוקש לא נמצא.");
        }

        return fullPath;
    }
}
