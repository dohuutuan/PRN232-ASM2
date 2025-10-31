using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace FUNewsManagement_FE.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginInput Input { get; set; } = new();

        public string? AlertMessage { get; set; }

        public class LoginInput
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(Input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                response = await client.PostAsync("https://localhost:7244/api/auth/login", content);
            }
            catch
            {
                AlertMessage = "Không thể kết nối tới máy chủ. Vui lòng thử lại sau.";
                return Page();
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            // ❌ Login thất bại
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    using var doc = JsonDocument.Parse(responseBody);
                    if (doc.RootElement.TryGetProperty("message", out var message))
                    {
                        AlertMessage = message.GetString() ?? "Đăng nhập thất bại.";
                    }
                    else
                    {
                        AlertMessage = "Email hoặc mật khẩu không đúng!";
                    }
                }
                catch
                {
                    AlertMessage = response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                        ? "Email hoặc mật khẩu không đúng!"
                        : "Server đang lỗi, vui lòng thử lại sau.";
                }

                return Page();
            }

            // ✅ Login thành công
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var data = doc.RootElement.GetProperty("data");

                var accessToken = data.GetProperty("accessToken").GetString();
                var refreshToken = data.GetProperty("refreshToken").GetString();

                HttpContext.Session.SetString("access_token", accessToken!);
                HttpContext.Session.SetString("refresh_token", refreshToken!);

                // ✅ Chuyển sang dashboard
                return RedirectToPage("/Analytics/Dashboard");
            }
            catch
            {
                AlertMessage = "Phản hồi từ máy chủ không hợp lệ.";
                return Page();
            }
        }
    }
}
