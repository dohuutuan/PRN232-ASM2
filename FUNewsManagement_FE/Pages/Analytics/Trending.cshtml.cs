using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace FUNewsManagement_FE.Pages.Analytics
{
    public class TrendingModel : PageModel
    {
        public class TrendingArticle
        {
            public int NewsArticleId { get; set; }
            public string NewsTitle { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string Author { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }
        }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TrendingModel> _logger;

        public TrendingModel(IHttpClientFactory httpClientFactory, ILogger<TrendingModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public List<TrendingArticle> TrendingArticles { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://localhost:7053");

                var response = await client.GetAsync("/api/analytics/trending");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<TrendingArticle>>();
                    TrendingArticles = data ?? new();
                }
                else
                {
                    ErrorMessage = $"Không thể tải dữ liệu trending ({response.StatusCode})";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trending articles");
                ErrorMessage = $"Không thể tải dữ liệu trending. Chi tiết: {ex.Message}";
            }
        }
    }
}
