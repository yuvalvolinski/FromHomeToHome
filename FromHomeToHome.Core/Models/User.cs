using System;

namespace FromHomeToHome.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // שמירת סיסמה מוצפנת
        public int CurrentLevel { get; set; } = 1;              // מתחיל ברמה 1 (ישראל)
        public int TotalScore { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
