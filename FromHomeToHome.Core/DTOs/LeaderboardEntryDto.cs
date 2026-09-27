namespace FromHomeToHome.Core.DTOs
{
    public class LeaderboardEntryDto
    {
        public string Username { get; set; } = string.Empty;
        public int TotalScore { get; set; }
        public int CurrentLevel { get; set; }
    }
}
