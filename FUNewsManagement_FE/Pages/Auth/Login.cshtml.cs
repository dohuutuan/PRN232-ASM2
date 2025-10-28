using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace FUNewsManagement_FE.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public LoginModel(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        public ApiResponse? LoginResult { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var email = Request.Form["Email"].ToString();
            var password = Request.Form["Password"].ToString();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin.";
                return Page();
            }

            try
            {
                var apiBase = _config["ApiSettings:BaseUrl"];
                if (string.IsNullOrEmpty(apiBase))
                {
                    ViewData["ErrorMessage"] = "Chưa cấu hình ApiSettings:BaseUrl trong appsettings.json.";
                    return Page();
                }

                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(apiBase);

                var response = await client.PostAsJsonAsync("/api/auth/login", new
                {
                    Email = email,
                    Password = password
                });

                if (!response.IsSuccessStatusCode)
                {
                    ViewData["ErrorMessage"] = "Email hoặc mật khẩu không đúng.";
                    return Page();
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                if (result?.Data == null || string.IsNullOrEmpty(result.Data.AccessToken))
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    ViewData["ErrorMessage"] = "Không nhận được phản hồi hợp lệ từ máy chủ. Raw: " + raw;
                    return Page();
                }

                // ✅ Gửi dữ liệu qua View để render JavaScript lưu localStorage
                LoginResult = result;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Không thể kết nối tới API: " + ex.Message;
                return Page();
            }
        }

        // DTO
        public class ApiResponse
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public LoginData? Data { get; set; }
        }

        public class LoginData
        {
            [JsonPropertyName("accessToken")]
            public string? AccessToken { get; set; }

            [JsonPropertyName("refreshToken")]
            public string? RefreshToken { get; set; }

            [JsonPropertyName("account")]
            public AccountDTO? Account { get; set; }
        }

        public class AccountDTO
        {
            [JsonPropertyName("email")]
            public string? Email { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("role")]
            public int Role { get; set; }
        }
    }
}
