namespace FromHomeToHome.Core.DTOs
{
    // Sent from server to client at the start of a round. No real coordinates included.
    public class LocationForGameDto
    {
        public int LocationId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int LevelNumber { get; set; }
    }
}
