using OpenCvSharp;

namespace CameraApi.Interfaces
{
    public interface ICamera
    {
        bool Running { get; }

        Task<Mat> GetMatImage();
        Task StartCamera();
        Task StopCamera();
        Task<bool> CheckCamera();

    }
}
