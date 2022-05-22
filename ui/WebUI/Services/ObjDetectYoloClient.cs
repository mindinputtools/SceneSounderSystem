using Yolov5Net.Scorer;

namespace WebUI.Services
{
    public class ObjDetectYoloClient
    {
        private readonly HttpClient httpClient;

        public ObjDetectYoloClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<YoloPrediction>> YoloPredictions(bool speak = false)
        {
            string requestUri = $"/api/objdetect/yolo/?speak={speak}";
            return await httpClient.GetFromJsonAsync<IEnumerable<YoloPrediction>>(requestUri);
        }
        public async Task StartYolo()
        {
            string requestUri = $"/api/objdetect/yolo/start";
            await httpClient.GetAsync(requestUri);
        }
        public async Task StopYolo()
        {
            string requestUri = $"/api/objdetect/yolo/stop";
            await httpClient.GetAsync(requestUri);
        }

    }
}
