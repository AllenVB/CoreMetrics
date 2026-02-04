using Microsoft.EntityFrameworkCore;
using SimpleAnalytics.Api.Data;      // AppDbContext'i bulabilmesi için gerekli
using SimpleAnalytics.Api.Services;  // IAnalyticsService ve AnalyticsService için gerekli

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný Baðlantýsý: appsettings.json içindeki þifre ve host bilgilerini kullanarak PostgreSQL'e baðlanýr
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Servis Kaydý: Uygulama içinde veritabaný iþlemlerini yapacak olan servisimizi sisteme tanýtýyoruz
// Bu satýr sayesinde Controller içerisinde IAnalyticsService kullanabileceðiz.
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddControllers();

// Swagger (Test arayüzü) yapýlandýrmasý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- SaaS projesi için önemli: Farklý sitelerden veri alabilmek için CORS ayarý ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() // Tüm adreslere izin ver
                  .AllowAnyMethod() // Tüm metodlara (GET, POST) izin ver
                  .AllowAnyHeader(); // Tüm baþlýklara izin ver
        });
});

var app = builder.Build();

// Geliþtirme aþamasýnda Swagger'ý aktif et (Tarayýcýdan test etmek için)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS politikasýný devreye al
app.UseCors("AllowAll");

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Yetkilendirme (Ýleride üyelik sistemi eklediðinde lazým olacak)
app.UseAuthorization();

// Ýstekleri ilgili Controller'lara yönlendir
app.MapControllers();

app.Run();