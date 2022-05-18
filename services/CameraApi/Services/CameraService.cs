using CameraApi.Interfaces;

namespace CameraApi.Services
{
    public class CameraService
    {
        private readonly ICamera camera;

        public CameraService(ICamera camera)
        {
            this.camera = camera;
        }
        public async Task<bool> Check()
        {
            return await camera.CheckCamera();
        }
        public async Task Start()
        {
            await camera.StartCamera();
        }
        public async Task Stop()
        {
            await camera.StopCamera();
        }
        public async Task<IResult> GetImage()
        {
            if (!camera.Running) await camera.StartCamera();
            var mat = await camera.GetMatImage();
            if (mat.Empty())
            { // We need to retry. Can happen when camera just started.
                int retryCount = 0;
                do
                {
                    mat = await camera.GetMatImage();
                } while (mat.Empty() && retryCount++ <3);
                if (mat.Empty()) throw new Exception("Cannot get image from camera after retries..");
            }
            var bytes = mat.ToBytes();
            return Results.File(bytes, "image/png");
        }
        public bool Running() => camera.Running;
    }
}
