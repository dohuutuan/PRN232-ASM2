using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class TokenService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IHttpClientFactory clientFactory, IHttpContextAccessor accessor)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = accessor;
    }

    public async Task<string?> GetValidAccessTokenAsync()
    {
        var session = _httpContextAccessor.HttpContext!.Session;
        var accessToken = session.GetString("access_token");
        var refreshToken = session.GetString("refresh_token");

        // (Giả sử bạn có cách lưu/giải mã JWT để kiểm tra expiration)
        //if (!JwtHelper.IsTokenExpired(accessToken))
        //    return accessToken;

        // Refresh nếu hết hạn
        var client = _clientFactory.CreateClient();
        var data = new { refreshToken };
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://yourapi.com/api/auth/refresh", content);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(result);
        var newAccessToken = doc.RootElement.GetProperty("token").GetString();
        var newRefreshToken = doc.RootElement.GetProperty("refreshToken").GetString();

        session.SetString("access_token", newAccessToken!);
        session.SetString("refresh_token", newRefreshToken!);

        return newAccessToken;
    }
}
