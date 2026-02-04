using Microsoft.AspNetCore.Mvc;
using SimpleAnalytics.Api.Services;

namespace SimpleAnalytics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public DashboardController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // ApiKey'e göre sitenin özet istatistiklerini getirir
        [HttpGet("summary/{apiKey}")]
        public async Task<IActionResult> GetSummary(string apiKey)
        {
            var summary = await _analyticsService.GetWebsiteSummary(apiKey);
            if (summary == null) return NotFound("Geçersiz API Key.");

            return Ok(summary);
        }
    }
}