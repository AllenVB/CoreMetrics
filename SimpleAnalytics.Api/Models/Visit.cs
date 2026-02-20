using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleAnalytics.Api.Models;

public class Visit
{
    public long Id { get; set; }
    public int WebsiteId { get; set; }
    public string Path { get; set; }
    public string Referrer { get; set; }
    public string UserAgent { get; set; }

    [NotMapped]  // ← EKLE: DB'ye yazılmaz, sadece servis içinde kullanılır
    public string IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
}
