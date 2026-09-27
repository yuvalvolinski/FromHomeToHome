namespace FromHomeToHome.Core.Models
{
    public class Level
    {
        public int LevelNumber { get; set; }                  // למשל: 1 (ישראל), 2 (דרום אירופה)...
        public string Name { get; set; } = string.Empty;      // שם האזור
        public string Description { get; set; } = string.Empty;
        public int RequiredScoreToUnlock { get; set; }       // סף נקודות למעבר לרמה הבאה
    }
}
