using SpeechClient.Models;
using System.Net.Http.Json;

namespace SpeechClient
{
    public class Speaker
    {
        
        private readonly Uri apiBaseAddress;
        internal static readonly HttpClient httpClientShared = new HttpClient();

        public Speaker(Uri apiAddress)
        {
            
            this.apiBaseAddress = apiAddress;
            httpClientShared.BaseAddress = apiAddress;
        }
        public async Task<Guid> SpeakText(string text)
        {
            
            var response = await httpClientShared.PostAsJsonAsync("/api/speech", new SpeakText() { Text = text });
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"SpeechClient error: returned {response.StatusCode}, {response.ReasonPhrase}");
                return Guid.Empty;
            }
            var result = await response.Content.ReadAsStringAsync();
            return Guid.Parse(result);
        }
    }
}
