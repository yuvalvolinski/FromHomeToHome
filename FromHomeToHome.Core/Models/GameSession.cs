using System;

namespace FromHomeToHome.Core.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public double GuessedLat { get; set; }
        public double GuessedLng { get; set; }
        public double DistanceMeters { get; set; }            // מרחק שחושב בשרת
        public int ScoreEarned { get; set; }                  // ניקוד שהושג בסיבוב
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    }
}
