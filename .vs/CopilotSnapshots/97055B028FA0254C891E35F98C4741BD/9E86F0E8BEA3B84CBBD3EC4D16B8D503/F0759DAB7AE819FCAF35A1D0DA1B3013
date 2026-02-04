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

        // GÖREV 1: Bir ziyareti PostgreSQL'e kaydetmek
        public async Task<bool> TrackVisit(Visit visit)
        {
            // Hataları gizlemek yerine logla ve yukarı fırlat
            try
            {
                await _context.Visits.AddAsync(visit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Visit kaydı sırasında hata oluştu. WebsiteId: {WebsiteId}, Path: {Path}", visit?.WebsiteId, visit?.Path);
                throw; // Hata kontrolü için üst katmana fırlatıyoruz
            }
        }

        // GÖREV 2: Yeni bir web sitesi kaydetmek ve ApiKey üretmek
        public async Task<Website> RegisterWebsite(string name, string domain)
        {
            var website = new Website
            {
                Name = name,
                Domain = domain,
                ApiKey = Guid.NewGuid().ToString() // Benzersiz bir anahtar üretir
            };

            _context.Websites.Add(website);
            await _context.SaveChangesAsync();
            return website;
        }

        // GÖREV 3: ApiKey kullanarak siteyi doğrulamak
        public async Task<Website?> GetWebsiteByApiKey(string apiKey)
        {
            return await _context.Websites
                .FirstOrDefaultAsync(w => w.ApiKey == apiKey);
        }

        // GÖREV 4: Ham istatistikleri çekmek
        public async Task<List<Visit>> GetStats(int websiteId)
        {
            return await _context.Visits
                .Where(v => v.WebsiteId == websiteId)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        // YENİ GÖREV (EKLEMEN GEREKEN): Dashboard için özet istatistikler
        public async Task<object> GetWebsiteSummary(string apiKey)
        {
            var website = await GetWebsiteByApiKey(apiKey);
            if (website == null) return null;

            var totalVisits = await _context.Visits.CountAsync(v => v.WebsiteId == website.Id);

            // En çok ziyaret edilen 5 sayfayı gruplayıp sayıyoruz
            var topPages = await _context.Visits
                .Where(v => v.WebsiteId == website.Id)
                .GroupBy(v => v.Path)
                .Select(g => new { Path = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            return new
            {
                WebsiteName = website.Name,
                TotalVisits = totalVisits,
                TopPages = topPages
            };
        }
    }
}