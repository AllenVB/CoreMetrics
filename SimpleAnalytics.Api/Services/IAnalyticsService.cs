using SimpleAnalytics.Api.Models; // Website modelini tanımak için

namespace SimpleAnalytics.Api.Services
{
    // IAnalyticsService bir "Arayüz"dür (Interface). 
    // İçine kod yazılmaz, sadece hangi işlerin yapılacağı (metot imzaları) yazılır.
    // Services/IAnalyticsService.cs
    public interface IAnalyticsService
    {
        Task<bool> TrackVisit(Visit visit);
        Task<Website?> GetWebsiteByApiKey(string apiKey);
        Task<Website> RegisterWebsite(string name, string domain); // Bu satır tam olarak böyle mi?
        Task<List<Visit>> GetStats(int websiteId);
        // Sitenin genel istatistik özetini getirir
        Task<object> GetWebsiteSummary(string apiKey);
    }
}