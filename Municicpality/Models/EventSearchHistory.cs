namespace Municicpality.Models
{
    // Model to track user search patterns for recommendation feature
    public class EventSearchHistory
    {
        public int Id { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime SearchDate { get; set; }
        public string UserSession { get; set; } = string.Empty; // Simple session tracking
    }
}
