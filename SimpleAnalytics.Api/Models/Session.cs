namespace SimpleAnalytics.Api.Models;

public class Session
{
    public long Id { get; set; }
    public int WebsiteId { get; set; }
    public int Duration { get; set; }   // Saniye cinsinden oturum süresi
    public string? Path { get; set; }
    public DateTime CreatedAt { get; set; }
}

