namespace FUNewsManagement_CoreAPI.DTOs.Analytics
{
    public class ExportArticleDTO
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public bool NewsStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
