using System.Collections.Concurrent;

namespace SimpleAnalytics.Api.Services
{
    /// <summary>
    /// Bağlı SSE istemcilerini yöneten singleton servis.
    /// Yeni ziyaret kaydedildiğinde tüm bağlı dashboard'lara anlık bildirim gönderir.
    /// </summary>
    public class SseConnectionManager
    {
        private readonly ConcurrentDictionary<string, HttpResponse> _clients = new();

        public void AddClient(string clientId, HttpResponse response)
        {
            _clients.TryAdd(clientId, response);
        }

        public void RemoveClient(string clientId)
        {
            _clients.TryRemove(clientId, out _);
        }

        /// <summary>
        /// Tüm bağlı istemcilere "visit" olayı gönderir.
        /// </summary>
        public async Task NotifyAll(string eventName = "visit", string data = "{}")
        {
            var deadClients = new List<string>();

            foreach (var (clientId, response) in _clients)
            {
                try
                {
                    await response.WriteAsync($"event: {eventName}\ndata: {data}\n\n");
                    await response.Body.FlushAsync();
                }
                catch
                {
                    // İstemci bağlantıyı kesmiş
                    deadClients.Add(clientId);
                }
            }

            foreach (var id in deadClients)
                RemoveClient(id);
        }

        public int ConnectionCount => _clients.Count;
    }
}

