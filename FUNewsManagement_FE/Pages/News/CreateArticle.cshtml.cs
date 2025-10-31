//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//public class CreateArticleModel : PageModel
//{
//    private readonly GeminiService _geminiService;

//    public CreateArticleModel(GeminiService geminiService)
//    {
//        _geminiService = geminiService;
//    }

//    [BindProperty]
//    public string Title { get; set; }

//    [BindProperty]
//    public string Content { get; set; }

//    [BindProperty]
//    public List<string> Tags { get; set; } = new List<string>();

//    [BindProperty]
//    public List<string> SuggestedTags { get; set; } = new List<string>();

//    public void OnGet()
//    {
//    }

//    public async Task<IActionResult> OnPostSuggestTagsAsync()
//    {
//        if (string.IsNullOrWhiteSpace(Content))
//        {
//            return new JsonResult(new { success = false, tags = Array.Empty<string>() });
//        }

//        var tags = await _geminiService.GetTagSuggestionsAsync(Content);

//        return new JsonResult(new { success = true, tags });
//    }

//    public IActionResult OnPostSaveArticle()
//    {
//        // Lưu bài viết vào DB (ví dụ)
//        // Title, Content, Tags đã có trong BindProperty

//        TempData["Message"] = "Bài viết đã được lưu!";
//        return RedirectToPage();
//    }
//}
