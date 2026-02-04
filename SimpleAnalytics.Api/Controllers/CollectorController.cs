using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SimpleAnalytics.Api.Models;
using SimpleAnalytics.Api.Services;

namespace SimpleAnalytics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Tarayýcýdan gelen isteklere izin vermek için (Program.cs'deki isimle ayný olmalý)
    [EnableCors("AllowAll")]
    public class CollectorController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        // Dependency Injection ile Servisi içeri alýyoruz
        public CollectorController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpPost("track")]
        public async Task<IActionResult> Track([FromBody] VisitDto data)
        {
            // 1. ADIM: Gelen ApiKey ile websitesini doðrula
            var website = await _analyticsService.GetWebsiteByApiKey(data.ApiKey);

            if (website == null)
            {
                return Unauthorized("Geçersiz API Key.");
            }

            // 2. ADIM: JavaScript'ten gelen (DTO) veriyi, Veritabaný Modeline (Visit) aktar
            // Burada 'Browser' veritabanýndaki sütun adýdýr, 'UserAgent' ise JS'den gelen veridir.
            var visit = new Visit
            {
                WebsiteId = website.Id,
                Path = data.Path,
                Referrer = data.Referrer,
                UserAgent = data.UserAgent, // Düzeltildi: Visit sýnýfýndaki property 'UserAgent'
                CreatedAt = DateTime.UtcNow
            };

            // 3. ADIM: Servis üzerinden kaydý tamamla
            await _analyticsService.TrackVisit(visit);

            return Ok(new { status = "success" });
        }
    }

    /// <summary>
    /// JavaScript'ten gelen JSON paketini karþýlayan sýnýftýr.
    /// JSON anahtarlarý ile buradaki isimler eþleþmelidir.
    /// </summary>
    public class VisitDto
    {
        public string ApiKey { get; set; }
        public string Path { get; set; }
        public string Referrer { get; set; }
        public string UserAgent { get; set; } // Bu isim Controller içindeki 'data.UserAgent' ile ayný olmalý
    }
}