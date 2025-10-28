namespace FUNewsManagement_CoreAPI.DTOs.Analytics
{
    public class TrendingDTO
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
