using FUNewsManagement_AnalyticsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_AnalyticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AnalyticsCacheService _cacheService;
        public AnalyticsController(HttpClient httpClient, AnalyticsCacheService cacheService)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardAsync()
        {
            var json = await _cacheService.GetDashboardAsync();
            return Content(json, "application/json");
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingAsync()
        {
            var json = await _cacheService.GetTrendingAsync();
            return Content(json, "application/json");
        }

        // ✅ GET /api/analytics/export
        [HttpGet("export")]
        public async Task<IActionResult> ExportAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7244/api/analytics/export");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, $"Lỗi khi gọi API: {response.ReasonPhrase}");

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = "Analytics.xlsx";
                return File(fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

    }
}
