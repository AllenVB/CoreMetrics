📊 CoreMetrics: SaaS Analytics Dashboard
CoreMetrics, web siteleri için geliştirilmiş, hafif (lightweight) ve gerçek zamanlı bir ziyaretçi takip sistemidir. Bu proje, bir SaaS (Software as a Service) modeli olarak kurgulanmış olup, birden fazla web sitesinden gelen verileri merkezi bir panelde analiz etmeyi sağlar.

🚀 Öne Çıkan Özellikler
Gerçek Zamanlı Veri Akışı: Vercel üzerinde yayında olan sitelerden gelen verileri Ngrok tüneli üzerinden anlık olarak yakalar ve işler.

Multi-Tenant Yapı: Farklı web sitelerini ApiKey tabanlı yetkilendirme ile birbirinden ayırır.

Detaylı Analiz: Toplam ziyaret, benzersiz sayfa görüntülemeleri ve sayfa bazlı dağılımı görsel grafiklerle sunar.

Güvenlik: API seviyesinde Unauthorized (401) kontrolü ve CORS politikalarıyla veri güvenliğini sağlar.

🛠️ Teknik Mimari ve Teknoloji Yığını
Bu proje, modern yazılım mimarisi prensipleri ve Yazılım Mühendisliği 2025-2026 müfredatı kapsamında öğrenilen teorik bilgilerin pratik uygulamasıdır:

Backend: C#, .NET 8 Web API ve Entity Framework Core.

Frontend: HTML5, Tailwind CSS, JavaScript (ES6+) ve veri görselleştirme için Chart.js.

Veritabanı: PostgreSQL. Veriler, 2.NF ve 3.NF normalizasyon kurallarına uygun olarak modellenmiştir.

DevOps: Ngrok (Local-to-Web Tunneling) ve Vercel (Cloud Deployment).

📖 Öğrenim Çıktıları
Geliştirme süreci boyunca aşağıdaki konularda deneyim kazanılmıştır:

Yazılım Tasarımı ve Mimarisi: Katmanlı mimari (Layered Architecture) ve servis tabanlı yaklaşım.

Algoritma Analizi: Big O notasyonu çerçevesinde veri işleme optimizasyonu.

Bulut Mimarileri: IaaS, PaaS ve SaaS platform modelleri.

🔧 Kurulum
appsettings.json içindeki PostgreSQL bağlantı dizesini düzenleyin.

Update-Database komutunu çalıştırarak şemayı oluşturun.

Ngrok tünelini API portunuzda başlatın.
