using System.Net.Http;
using System.Text;
using System.Text.Json;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<List<string>> GetTagSuggestionsAsync(string content)
    {
        // Endpoint mới v1beta
        string endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

        // Body request theo spec mới (contents -> parts -> text)
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = $"Suggest 3-5 relevant tags (single words, comma-separated) for the following article content:\r\n{content}\r\n" }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Header theo doc
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

        var response = await _httpClient.PostAsync(endpoint, httpContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Gemini API lỗi: {response.StatusCode}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();

        // Parse output text từ "output_text" hoặc "parts" tuỳ response
        var doc = JsonDocument.Parse(responseJson);
        string output = "";

        if (doc.RootElement.TryGetProperty("results", out var results) && results.GetArrayLength() > 0)
        {
            var firstResult = results[0];

            if (firstResult.TryGetProperty("contents", out var contents) && contents.GetArrayLength() > 0)
            {
                var firstContent = contents[0];
                if (firstContent.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                {
                    var firstPart = parts[0];
                    if (firstPart.TryGetProperty("text", out var textElem))
                    {
                        output = textElem.GetString() ?? "";
                    }
                }
            }
        }

        // Tách tag theo dấu phẩy
        var tags = output.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(t => t.Trim())
                         .Where(t => t.Length > 0)
                         .Distinct()
                         .ToList();

        return tags;
    }
}
