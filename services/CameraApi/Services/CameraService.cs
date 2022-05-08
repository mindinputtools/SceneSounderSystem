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
        public bool Check()
        {
            return camera.CheckCamera();
        }
    }
}
