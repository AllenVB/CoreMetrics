using Microsoft.EntityFrameworkCore;
using SimpleAnalytics.Api.Models;

namespace SimpleAnalytics.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Website> Websites { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<Session> Sessions { get; set; } 
}

