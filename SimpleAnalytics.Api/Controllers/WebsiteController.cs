using Microsoft.AspNetCore.Mvc;
using SimpleAnalytics.Api.Models;
using SimpleAnalytics.Api.Services;

namespace SimpleAnalytics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public WebsiteController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // Yeni bir web sitesi kaydetmek için kullanılır
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] WebsiteRegistrationDto dto)
        {
            var website = await _analyticsService.RegisterWebsite(dto.Name, dto.Domain);
            return Ok(website); // Bu cevap bize ApiKey'i verecek!
        }
    }

    // İstek gönderirken kullanılacak basit veri yapısı
    public class WebsiteRegistrationDto
    {
        public string Name { get; set; }
        public string Domain { get; set; }
    }
}