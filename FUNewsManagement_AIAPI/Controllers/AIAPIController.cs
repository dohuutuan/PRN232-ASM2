using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AIAPIController : ControllerBase
{
    private readonly GeminiService _geminiService;

    public AIAPIController(GeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpGet("tag-suggest")]
    public async Task<IActionResult> GetTagSuggest([FromQuery] string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return BadRequest(new { success = false, message = "Content is empty", tags = Array.Empty<string>() });

        try
        {
            var tags = await _geminiService.GetTagSuggestionsAsync(content);
            return Ok(new { success = true, tags });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
