namespace CameraClient
{
    public class Camera
    {
        internal static readonly HttpClient httpClientShared = new HttpClient()
        {
            BaseAddress = new Uri("http://camera-api:8080") // hard coded for now
        };

        public async Task<Stream> GetImageStreamAsync()
        {
            var stream = await httpClientShared.GetStreamAsync("/api/camera/image");
            return stream;
        }
    }
}
