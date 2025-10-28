using FUNewsManagement_CoreAPI.Models;

namespace FUNewsManagement_CoreAPI.Repositories.Interfaces
{
    public interface IAnalyticsRepo
    {
        Task<object> GetDashboardAsync();
        Task<IEnumerable<object>> GetTrendingAsync(int top);
        Task<IEnumerable<object>> GetRecommendAsync(string articleId);
        Task<IEnumerable<object>> GetAllForExportAsync();
    }
}
