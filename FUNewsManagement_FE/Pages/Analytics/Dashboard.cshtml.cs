using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FUNewsManagement_FE.Pages.Analytics
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public DashboardData? AnalyticsData { get; set; }
        public List<string> Categories { get; set; } = new();
        public List<string> Authors { get; set; } = new();
        public string? ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        public class DashboardData
        {
            public int TotalArticles { get; set; }
            public int Published { get; set; }
            public int Draft { get; set; }
            public int Pending { get; set; }
            public Dictionary<string, int> CategoryStats { get; set; } = new();
            public Dictionary<string, int> AuthorStats { get; set; } = new();
        }

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = _httpContextAccessor.HttpContext?.Session.GetString("access_token");

                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string url = "https://localhost:7053/api/analytics/dashboard";

                // Gắn query params (filter)
                var query = new List<string>();
                if (StartDate != null) query.Add($"startDate={StartDate:yyyy-MM-dd}");
                if (EndDate != null) query.Add($"endDate={EndDate:yyyy-MM-dd}");
                if (!string.IsNullOrEmpty(Category)) query.Add($"category={Category}");
                if (!string.IsNullOrEmpty(Status)) query.Add($"status={Status}");
                if (query.Any()) url += "?" + string.Join("&", query);

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = "Không thể tải dữ liệu dashboard.";
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                AnalyticsData = JsonSerializer.Deserialize<DashboardData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                ErrorMessage = "Lỗi khi gọi API analytics.";
            }
        }

        public IActionResult OnGetExport()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("access_token");
            var exportUrl = "https://localhost:7053/api/analytics/export";
            return Redirect(exportUrl + "?token=" + token);
        }
    }
}
