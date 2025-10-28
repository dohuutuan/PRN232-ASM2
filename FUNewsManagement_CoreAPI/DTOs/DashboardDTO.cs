namespace FUNewsManagement_CoreAPI.DTOs.Analytics
{
    public class DashboardDTO
    {
        public int TotalArticles { get; set; }
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
        public List<CategorySummaryDTO> ByCategory { get; set; } = new();
    }

    public class CategorySummaryDTO
    {
        public string Category { get; set; } = string.Empty;
        public int TotalArticles { get; set; }
        public int Active { get; set; }
        public int Inactive { get; set; }
    }
}
