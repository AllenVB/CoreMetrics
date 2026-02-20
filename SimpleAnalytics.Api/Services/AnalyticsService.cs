using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleAnalytics.Api.Data;
using SimpleAnalytics.Api.Models;

namespace SimpleAnalytics.Api.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AnalyticsService> _logger;

        public AnalyticsService(AppDbContext context, ILogger<AnalyticsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> TrackVisit(Visit visit)
        {
            // Güvenlik ağı
            if (visit.CreatedAt == default) visit.CreatedAt = DateTime.UtcNow;
            if (visit.WebsiteId == 0) return false;

            try
            {
                try
                {
                    using var client = new HttpClient();
                    client.Timeout = TimeSpan.FromSeconds(2);
                    var response = await client.GetFromJsonAsync<IpApiResponse>(
                        $"http://ip-api.com/json/{visit.IpAddress}");
                    if (response?.status == "success")
                    {
                        visit.Country = response.country;
                        visit.City = response.city;
                    }
                }
                catch { /* Lokasyon bulunamadıysa boş geç */ }

                await _context.Set<Visit>().AddAsync(visit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ziyaret kaydedilemedi!");
                return false;
            }
        }

        // ─── YENİ: Oturum süresi kaydet ────────────────────────────────────────
        public async Task<bool> TrackSession(Session session)
        {
            if (session.CreatedAt == default) session.CreatedAt = DateTime.UtcNow;
            if (session.WebsiteId == 0) return false;

            try
            {
                await _context.Sessions.AddAsync(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oturum kaydedilemedi!");
                return false;
            }
        }

        public async Task<Website> RegisterWebsite(string name, string domain)
        {
            var website = new Website
            {
                Name = name,
                Domain = domain,
                ApiKey = Guid.NewGuid().ToString()
            };
            _context.Websites.Add(website);
            await _context.SaveChangesAsync();
            return website;
        }

        public async Task<Website?> GetWebsiteByApiKey(string apiKey)
        {
            return await _context.Websites
                .FirstOrDefaultAsync(w => w.ApiKey == apiKey);
        }

        public async Task<List<Visit>> GetStats(int websiteId)
        {
            return await _context.Visits
                .Where(v => v.WebsiteId == websiteId)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<object> GetWebsiteSummary(string apiKey, int days = 7)
        {
            var website = await GetWebsiteByApiKey(apiKey);
            if (website == null) return null;

            var startDate = DateTime.UtcNow.AddDays(-days);

            var totalVisits = await _context.Visits
                .CountAsync(v => v.WebsiteId == website.Id && v.CreatedAt >= startDate);

            var topPages = await _context.Visits
                .Where(v => v.WebsiteId == website.Id && v.CreatedAt >= startDate)
                .GroupBy(v => v.Path)
                .Select(g => new PageStatDto { Path = g.Key ?? "Unknown", Count = g.Count() })
                .OrderByDescending(x => x.Count).Take(10).ToListAsync();

            var topLocations = await _context.Visits
                .Where(v => v.WebsiteId == website.Id && v.CreatedAt >= startDate && v.Country != null)
                .GroupBy(v => new { v.Country, v.City })
                .Select(g => new
                {
                    Country = g.Key.Country ?? "Global",
                    City = g.Key.City ?? "Bilinmeyen",
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count).Take(10).ToListAsync();

            // ─── YENİ: Oturum istatistikleri ────────────────────────────────
            var sessions = await _context.Sessions
                .Where(s => s.WebsiteId == website.Id && s.CreatedAt >= startDate)
                .ToListAsync();

            var avgSessionDuration = sessions.Count > 0
                ? (int)sessions.Average(s => s.Duration)
                : 0;

            return new
            {
                WebsiteName = website.Name,
                TotalVisits = totalVisits,
                TotalSessions = sessions.Count,
                AvgSessionDuration = avgSessionDuration,   // saniye
                TopPages = topPages,
                TopLocations = topLocations,
                PeriodDays = days
            };
        }

        public class IpApiResponse
        {
            public string status { get; set; }
            public string country { get; set; }
            public string city { get; set; }
        }
    }
}

