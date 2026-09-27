using System;

namespace FromHomeToHome.Core.DTOs
{
    // Sent from client to server with the player's guess
    public class SubmitGuessDto
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public double GuessedLat { get; set; }
        public double GuessedLng { get; set; }
        public DateTime GuessedAt { get; set; } = DateTime.UtcNow;
    }
}
