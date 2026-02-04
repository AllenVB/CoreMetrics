using SimpleAnalytics.Api.Models;

namespace SimpleAnalytics.Api.Models;

public class Visit
{
    public long Id { get; set; }
    public int WebsiteId { get; set; }
    public string Path { get; set; }
    public string Referrer { get; set; }
    public string UserAgent { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}