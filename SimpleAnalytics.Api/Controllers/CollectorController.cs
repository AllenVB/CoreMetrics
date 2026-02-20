using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SimpleAnalytics.Api.Models;
using SimpleAnalytics.Api.Services;

namespace SimpleAnalytics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class CollectorController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly SseConnectionManager _sseManager;

        public CollectorController(IAnalyticsService analyticsService, SseConnectionManager sseManager)
        {
            _analyticsService = analyticsService;
            _sseManager = sseManager;
        }

        [HttpPost("track")]
        public async Task<IActionResult> Track([FromBody] VisitDto data)
        {
            var website = await _analyticsService.GetWebsiteByApiKey(data.ApiKey);
            if (website == null)
                return Unauthorized("Geçersiz API Key.");

            var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                            ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            var visit = new Visit
            {
                WebsiteId = website.Id,
                Path = data.Path,
                Referrer = data.Referrer,
                UserAgent = data.UserAgent,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };

            await _analyticsService.TrackVisit(visit);
            await _sseManager.NotifyAll("visit", $"{{\"path\":\"{data.Path}\"}}");

            return Ok(new { status = "success" });
        }

        [HttpPost("session")]
        public async Task<IActionResult> TrackSession([FromBody] SessionDto data)
        {
            var website = await _analyticsService.GetWebsiteByApiKey(data.ApiKey);
            if (website == null)
                return Unauthorized("Geçersiz API Key.");

            if (data.Duration < 1 || data.Duration > 86400) // 1 sn ile 24 saat arasý
                return BadRequest("Geçersiz süre.");

            var session = new Session
            {
                WebsiteId = website.Id,
                Duration = data.Duration,
                Path = data.Path,
                CreatedAt = DateTime.UtcNow
            };

            await _analyticsService.TrackSession(session);
            return Ok(new { status = "ok" });
        }

        [HttpGet("live")]
        public async Task Live([FromQuery] string apiKey, CancellationToken cancellationToken)
        {
            var website = await _analyticsService.GetWebsiteByApiKey(apiKey);
            if (website == null) { Response.StatusCode = 401; return; }

            Response.Headers["Content-Type"] = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["X-Accel-Buffering"] = "no";
            Response.Headers["Connection"] = "keep-alive";

            var clientId = Guid.NewGuid().ToString();
            _sseManager.AddClient(clientId, Response);

            await Response.WriteAsync($"event: connected\ndata: {{\"clientId\":\"{clientId}\"}}\n\n");
            await Response.Body.FlushAsync();

            try { await Task.Delay(Timeout.Infinite, cancellationToken); }
            catch (TaskCanceledException) { }
            finally { _sseManager.RemoveClient(clientId); }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string apiKey, [FromQuery] int days = 7)
        {
            if (string.IsNullOrEmpty(apiKey))
                return BadRequest("apiKey zorunludur.");

            var summary = await _analyticsService.GetWebsiteSummary(apiKey, days);
            if (summary == null)
                return Unauthorized("Geçersiz API Key.");

            return Ok(summary);
        }

        [HttpGet("visits")]
        public async Task<IActionResult> GetVisits([FromQuery] string apiKey)
        {
            var website = await _analyticsService.GetWebsiteByApiKey(apiKey);
            if (website == null)
                return Unauthorized("Geçersiz API Key.");

            var visits = await _analyticsService.GetStats(website.Id);
            return Ok(visits);
        }
    }

    public class VisitDto
    {
        public string ApiKey { get; set; }
        public string Path { get; set; }
        public string Referrer { get; set; }
        public string UserAgent { get; set; }
    }

    public class SessionDto
    {
        public string ApiKey { get; set; }
        public int Duration { get; set; }   // Saniye
        public string? Path { get; set; }
    }
}

