using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using FUNewsManagement_CoreAPI.Models;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly FunewsManagementContext _context;

        public AnalyticsController(FunewsManagementContext context)
        {
            _context = context;
        }

        // ✅ GET /api/analytics/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? category, [FromQuery] string? status)
        {
            var query = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(n => n.CreatedDate >= startDate);

            if (endDate.HasValue)
                query = query.Where(n => n.CreatedDate <= endDate);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(n => n.Category.CategoryName == category);

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Published") query = query.Where(n => n.NewsStatus == true);
                else if (status == "Draft") query = query.Where(n => n.NewsStatus == false);
            }

            var articles = await query.ToListAsync();

            var total = articles.Count;
            var published = articles.Count(a => a.NewsStatus == true);
            var draft = articles.Count(a => a.NewsStatus == false);
            var pending = 0; // giả sử chưa có trạng thái pending

            var categoryStats = articles
                .Where(a => a.Category != null)
                .GroupBy(a => a.Category.CategoryName)
                .ToDictionary(g => g.Key, g => g.Count());

            var authorStats = articles
                .Where(a => a.CreatedBy != null)
                .GroupBy(a => a.CreatedBy.AccountName)
                .ToDictionary(g => g.Key, g => g.Count());

            return Ok(new
            {
                totalArticles = total,
                published,
                draft,
                pending,
                categoryStats,
                authorStats
            });
        }

        // ✅ GET /api/analytics/trending
        [HttpGet("trending")]
        public async Task<IActionResult> GetTrending()
        {
            var trending = await _context.NewsArticles
                .OrderByDescending(n => n.CreatedDate)
                .Take(5)
                .Select(n => new
                {
                    n.NewsArticleId,
                    n.NewsTitle,
                    Category = n.Category.CategoryName,
                    Author = n.CreatedBy.AccountName,
                    n.CreatedDate
                })
                .ToListAsync();

            return Ok(trending);
        }

        // ✅ GET /api/analytics/export
        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var articles = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Articles");

            sheet.Cells[1, 1].Value = "Article ID";
            sheet.Cells[1, 2].Value = "Title";
            sheet.Cells[1, 3].Value = "Category";
            sheet.Cells[1, 4].Value = "Author";
            sheet.Cells[1, 5].Value = "Status";
            sheet.Cells[1, 6].Value = "Created Date";

            int row = 2;
            foreach (var a in articles)
            {
                sheet.Cells[row, 1].Value = a.NewsArticleId;
                sheet.Cells[row, 2].Value = a.NewsTitle;
                sheet.Cells[row, 3].Value = a.Category?.CategoryName;
                sheet.Cells[row, 4].Value = a.CreatedBy?.AccountName;
                sheet.Cells[row, 5].Value = a.NewsStatus == true ? "Published" : "Draft";
                sheet.Cells[row, 6].Value = a.CreatedDate?.ToString("yyyy-MM-dd");
                row++;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Analytics.xlsx");
        }
    }
}
