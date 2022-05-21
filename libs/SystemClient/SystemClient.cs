using System.Net.Http.Json;

namespace SystemClient
{
    public class SystemClient
    {
        internal static readonly HttpClient httpClientShared = new HttpClient()
        {
            BaseAddress = new Uri("http://system-api:8080") // hard coded for now
        };
        public async Task<bool> PowerOffAsync()
        {
            return await httpClientShared.GetFromJsonAsync<bool>("/api/system/poweroff");
        }
        public async Task<bool> RebootAsync()
        {
            return await httpClientShared.GetFromJsonAsync<bool>("/api/system/reboot");
        }
        public async Task<bool> SetAudioVolumeAsync(int vol)
        {
            var response = await httpClientShared.PostAsync($"/api/system/setaudiovolume/?vol={vol}", null);
            if (response.IsSuccessStatusCode) Console.WriteLine($"Set audio vol to {vol} succeeded");
            else Console.WriteLine($"Set audio vol to {vol} failed");
            return response.IsSuccessStatusCode;
        }

    }
}
