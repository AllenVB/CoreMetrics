// Bu satır, AppDbContext'in bu sınıfı bulmasını sağlar.
namespace SimpleAnalytics.Api.Models;

public class Website
{
    public int Id { get; set; }
    public string Name { get; set; } // Örn: "Benim Blogum"
    public string Domain { get; set; } // Örn: "blogum.com"
    public string ApiKey { get; set; } = Guid.NewGuid().ToString(); // Güvenlik için
}