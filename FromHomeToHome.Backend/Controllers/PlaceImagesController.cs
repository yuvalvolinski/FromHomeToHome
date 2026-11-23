using Microsoft.AspNetCore.Mvc;
using FromHomeToHome.Backend.Services;

namespace FromHomeToHome.Backend.Controllers;

[Route("api/place-images")]
[ApiController]
public class PlaceImagesController : ControllerBase
{
    private readonly IPlaceImageService _placeImageService;
    private readonly ILogger<PlaceImagesController> _logger;

    public PlaceImagesController(IPlaceImageService placeImageService, ILogger<PlaceImagesController> logger)
    {
        _placeImageService = placeImageService;
        _logger = logger;
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomImage()
    {
        try
        {
            // בשלב זה מחזירים מיקום אקראי אחד בלבד, בלי לוגיקת משחק מלאה.
            var image = await _placeImageService.GetRandomImageAsync();
            return Ok(image);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "אין מיקומים זמינים לשליפה.");
            return NotFound("לא נמצאו מיקומים זמינים.");
        }
    }

    [HttpGet("file/{locationId:int}")]
    public async Task<IActionResult> GetImageFile(int locationId)
    {
        try
        {
            // השרת מגיש את התמונה לפי מזהה מיקום כדי לשמור על הכנה ל-anti-cheat בהמשך.
            var imagePath = await _placeImageService.GetImagePathAsync(locationId);
            var contentType = GetContentType(imagePath);
            return PhysicalFile(imagePath, contentType);
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "התמונה עבור LocationId={LocationId} לא נמצאה.", locationId);
            return NotFound("התמונה המבוקשת לא נמצאה.");
        }
    }

    private static string GetContentType(string imagePath)
    {
        return Path.GetExtension(imagePath).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };
    }
}
