using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SimpleAnalytics.Api.Services;

namespace SimpleAnalytics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class DashboardController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public DashboardController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // ApiKey'e göre sitenin özet istatistiklerini getirir
        [HttpGet("summary/{apiKey}")]
        public async Task<IActionResult> GetSummary(string apiKey, [FromQuery] int days = 7)
        {
            // Veri çekme mantığı AnalyticsService içinde gerçekleşiyor.
            // Oraya 'days' parametresini gönderiyoruz.
            var summary = await _analyticsService.GetWebsiteSummary(apiKey, days);

            if (summary == null)
            {
                // Eğer veritabanında bu ApiKey ile eşleşen site yoksa 404 döner.
                return NotFound(new { message = "Web sitesi bulunamadı veya API anahtarı geçersiz." });
            }

            return Ok(summary);
        }
    }
}

