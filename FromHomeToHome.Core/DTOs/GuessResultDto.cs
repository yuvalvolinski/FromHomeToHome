namespace FromHomeToHome.Core.DTOs
{
    // Returned from server after calculating distance and score
    public class GuessResultDto
    {
        public double DistanceMeters { get; set; }
        public int ScoreEarned { get; set; }
        // Real coordinates (sent only after guess is evaluated)
        public double TrueLatitude { get; set; }
        public double TrueLongitude { get; set; }
        public string InfoStory { get; set; } = string.Empty; // educational text shown to player
    }
}
