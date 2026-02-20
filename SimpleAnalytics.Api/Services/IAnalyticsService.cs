using SimpleAnalytics.Api.Models;

namespace SimpleAnalytics.Api.Services;

public interface IAnalyticsService
{
    Task<bool> TrackVisit(Visit visit);
    Task<bool> TrackSession(Session session);       
    Task<Website> RegisterWebsite(string name, string domain);
    Task<Website?> GetWebsiteByApiKey(string apiKey);
    Task<List<Visit>> GetStats(int websiteId);
    Task<object> GetWebsiteSummary(string apiKey, int days = 7);
}

