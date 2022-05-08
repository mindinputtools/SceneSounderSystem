using OpenCvSharp;

namespace CameraApi.Interfaces
{
    public interface ICamera
    {
        Mat GetMatImage();
        void StartCamera();
        void StopCamera();
        bool CheckCamera();

    }
}
