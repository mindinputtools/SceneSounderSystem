using SpeechClient.Models;
using System.Net.Http.Json;

namespace SpeechClient
{
    public class Speaker
    {
        
        internal static readonly HttpClient httpClientShared = new HttpClient()
        {
            BaseAddress = new Uri("http://speech-api:8080") // hard coded for now
        };

        public Speaker()
        {
            
        }
        public async Task<string> SpeakText(string text)
        {
            
            var response = await httpClientShared.PostAsJsonAsync("/api/speech", new SpeakText() { Text = text });
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"SpeechClient error: returned {response.StatusCode}, {response.ReasonPhrase}");
                return string.Empty;
            }
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
