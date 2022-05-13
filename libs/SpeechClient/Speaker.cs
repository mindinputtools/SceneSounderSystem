using SpeechClient.Models;
using System.Net.Http.Json;

namespace SpeechClient
{
    public class Speaker
    {
        
        private readonly Uri apiEndpoint;
        private readonly HttpClient httpClient;

        public Speaker(Uri apiEndpoint, IHttpClientFactory httpClientFactory)
        {
            
            this.apiEndpoint = apiEndpoint;
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = apiEndpoint;
        }
        public async Task<Guid> SpeakText(string text)
        {
            
            var response = await httpClient.PostAsJsonAsync("/api/speech", new SpeakText() { Text = text });
            var result = await response.Content.ReadAsStringAsync();
            return Guid.Parse(result);
        }
    }
}
