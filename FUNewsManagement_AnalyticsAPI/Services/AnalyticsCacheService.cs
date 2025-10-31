using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace FUNewsManagement_AnalyticsAPI.Services
{
    public class AnalyticsCacheService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _baseUrl = "https://localhost:7244/api/analytics";

        public AnalyticsCacheService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClient = httpClientFactory.CreateClient();
            _cache = memoryCache;
        }

        public async Task<string> GetDashboardAsync()
        {
            if (_cache.TryGetValue("dashboard_cache", out string cachedData))
                return cachedData;

            var response = await _httpClient.GetAsync($"{_baseUrl}/dashboard");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            _cache.Set("dashboard_cache", json, TimeSpan.FromMinutes(5));
            return json;
        }

        public async Task<string> GetTrendingAsync()
        {
            if (_cache.TryGetValue("trending_cache", out string cachedData))
                return cachedData;

            var response = await _httpClient.GetAsync($"{_baseUrl}/trending");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            _cache.Set("trending_cache", json, TimeSpan.FromMinutes(5));
            return json;
        }

        // background worker gọi để refresh cache
        public async Task RefreshAllAsync()
        {
            await GetDashboardAsync();
            await GetTrendingAsync();
        }
    }
}
