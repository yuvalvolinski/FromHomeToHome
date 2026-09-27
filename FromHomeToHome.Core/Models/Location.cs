namespace FromHomeToHome.Core.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int LevelNumber { get; set; }                  // שיוך לשלב מתאים
        public string Name { get; set; } = string.Empty;      // שם המקום (למשל: "מבצר נמרוד")
        public string ImageUrl { get; set; } = string.Empty;  // קישור לתמונה
        public double Latitude { get; set; }                  // קו רוחב אמיתי
        public double Longitude { get; set; }                 // קו אורך אמיתי
        public string InfoStory { get; set; } = string.Empty; // הטקסט שיוצג לאחר הניחוש
    }
}
