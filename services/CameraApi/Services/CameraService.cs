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
            var bytes = mat.ToBytes();
            return Results.File(bytes, "image/png");
        }
        public bool Running() => camera.Running;
    }
}
