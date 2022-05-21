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
            var done = false;
            var retries =0;
            HttpResponseMessage response = new();
            do
            {
                try
                {
                    response = await httpClientShared.PostAsJsonAsync("/api/speech", new SpeakText() { Text = text });
                    done = true;
                }
                catch (HttpRequestException)
                {
                    if (retries++ > 3)
                    throw;
                    await Task.Delay(1000);
                }
                
            } while (!done);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"SpeechClient error: returned {response.StatusCode}, {response.ReasonPhrase}");
                return string.Empty;
            }
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
        public async Task<bool> StopAsync()
        {
            var response = await httpClientShared.GetAsync("/api/speech/?stop=true");
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
