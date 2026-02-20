# 📊 CoreMetrics - Gerçek Zamanlı Analitik ve Ziyaretçi Takip Sistemi

CoreMetrics, web sitelerinin ziyaretçi metriklerini **gerçek zamanlı (Real-time)** olarak izlemek, oturum sürelerini takip etmek, sayfa tıklamalarını analiz etmek ve kullanıcıların lokasyon verilerini toplamak için geliştirilmiş **sunucusuz (Serverless)** bir analitik platformudur. Google Analytics gibi ağır ve hantal sistemlerin aksine, hızlı, güvenli ve tamamen geliştirici kontrollüdür.


---<img width="1919" height="974" alt="core1" src="https://github.com/user-attachments/assets/ca31a3a5-96fd-4a31-9973-249b8eb9a257" />


## 🔥 Temel Özellikler

- **Gerçek Zamanlı Veri Akışı:** Ziyaretçi istatistiklerini websocket yerine HTTP tabanlı modern **SSE (Server-Sent Events)** teknolojisi ile anlık ve kesintisiz sunar.
- **Güvenli Oturum Takibi:** Kullanıcı sekmeyi veya tarayıcıyı kapatsa bile `navigator.sendBeacon()` API'si kullanılarak oturum süresi sunucuya %100 oranında iletilir.
- **Lokasyon Analizi:** IP adresleri üzerinden ziyaretçilerin ülke ve şehir bilgilerini haritalandırır.
- **API Key Koruması:** Frontend dashboard üzerinde API key güvenlik katmanı bulundurarak yetkisiz erişimleri engeller. Güvenli `localStorage` yönetimi sunar.
- **Serverless Mimari:** Frankfurt lokasyonlu **Google Cloud Run** üzerinde konumlandırılmış, otomatik ölçeklenebilir (auto-scaling) backend yapısına sahiptir.

---

## 🛠️ Kullanılan Teknolojiler

### Backend
* **C# / .NET Core**
* **Google Cloud Run** (Serverless Deployment)
* **PostgreSQL** (Veritabanı)

### Frontend (Dashboard)
* **Vanilla JavaScript**
* **HTML5 / CSS3**
* **Tailwind CSS** (Modern UI tasarımı ve Cam efekti - Glassmorphism)
* **Three.js** (Arka plan 3D animasyonları)
* **Chart.js** (Grafikleştirme)

---

## 🚀 Kurulum ve Entegrasyon Kılavuzu

CoreMetrics'i herhangi bir Vanilla JS, React, Vue veya svelte projesine saniyeler içinde entegre edebilirsiniz. Aşağıdaki adımları kendi web sayfanızda uygulamanız yeterlidir.

### 1. Sayfa Ziyaretlerini ve Tıklamaları İzleme
Sitenizin ana JavaScript dosyasına (örneğin [app.js](cci:7://file:///c:/Users/XXX/Desktop/Projects/XXX/app.js:0:0-0:0)) aşağıdaki konfigürasyon ve fonksiyonu ekleyin:

\`\`\`javascript
const CORE_CONFIG = {
    API_KEY: "SİZİN_GİZLİ_API_ANAHTARINIZ",
    BASE_URL: "https://coremetrics-service.xxxxxx.run.app/api/Collector"
};

// Sayfa geçişlerini yakalama fonksiyonu
async function trackCoreMetrics(path) {
    try {
        await fetch(CORE_CONFIG.BASE_URL + "/track", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                apiKey: CORE_CONFIG.API_KEY,
                path: path,
                referrer: document.referrer || "Doğrudan Giriş",
                userAgent: navigator.userAgent
            })
        });
    } catch (err) {
        console.warn("CoreMetrics: Bağlantı hatası.");
    }
}

// Projenizde sayfalar arası geçiş yaptığınız yerlerde çağırın:
// Örnek: trackCoreMetrics('/#hakkimda');
\`\`\`

### 2. Oturum Süresini (Session Duration) İzleme
Ziyaretçi sitenizden bağını tam olarak kopardığında (sekmeyi kapattığında) sitede geçirdiği net süreyi yakalamak için aşağıdaki kodu ekleyin. Bu kod, sayfa kapanırken bile çalışan `sendBeacon` sistemini kullanır.

\`\`\`javascript
const _sessionStart = Date.now();
const _sessionPath = window.location.hash || "/";

window.addEventListener("beforeunload", () => {
    const duration = Math.round((Date.now() - _sessionStart) / 1000);
    
    // 2 saniyeden kısa süren önemsiz ziyaretleri (bot veya yanlış tıklama) yoksay
    if (duration < 2) return; 

    navigator.sendBeacon(
        CORE_CONFIG.BASE_URL + "/session",
        new Blob([JSON.stringify({
            apiKey: CORE_CONFIG.API_KEY,
            duration: duration,
            path: _sessionPath
        })], { type: "application/json" })
    );
});
\`\`\`

### 3. Dashboard Kurulumu
1. Bu repodaki [dashboard.html](cci:7://file:///c:/Users/XXX/Desktop/Projects/XXX/dashboard.html:0:0-0:0) dosyasını kendi projenize kopyalayın.
2. [dashboard.html](cci:7://file:///c:/Users/XXX/Desktop/Projects/XXX/dashboard.html:0:0-0:0) içerisindeki `CONFIG` objesine backend URL'inizi tanımlayın.
   *(Güvenlik sebebiyle API Key'i kodun içine gömmeyin, sadece login ekranından girecek şekilde boş bırakın)*
3. İlgili dosyayı Vercel, Netlify veya herhangi bir statik sunucuda yayınlayın.

---

## 🔒 Güvenlik Notu
[dashboard.html](cci:7://file:///c:/Users/XXX/Desktop/Projects/XXX/dashboard.html:0:0-0:0) kaynak koduna asla API Key'inizi açık bir şekilde yazmayın. Bu projede, API Key tarayıcının `localStorage` (Yerel Depolama) hafızasında güvenle şifreli olarak tutulmakta ve sadece yetki verdiğiniz cihazlarda dashboarda erişim sağlanmaktadır.

## 👨‍💻 Geliştirici
**Süleyman Emre Arlı**  
*[LinkedIn](www.linkedin.com/in/suleymanemrearlii) • [GitHub](https://github.com/AllenVB)*
