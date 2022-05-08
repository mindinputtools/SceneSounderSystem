using CameraApi.Interfaces;
using OpenCvSharp;

namespace CameraApi.Services
{
    public class OpenCVCamera : ICamera
    {
        private VideoCapture _video;
        private bool _cameraRunning;
        private bool _saveFrame;
        private bool _gotFrame;
        private Action<Mat> GotFrame;
        public OpenCVCamera()
        {
            //            Console.WriteLine("Opening camera...");

        }
        private void cameraThread()
        {
            _cameraRunning = true;
            while (_cameraRunning)
            {
                _video.Grab();
                Task.Delay(200);
            }
            _video.Release();
        }
        public Mat GetMatImage()
        {
            Mat _curFrame = _cameraRunning ? _video.RetrieveMat() : new Mat();
            if (!_cameraRunning || _curFrame.Empty())
            {
                Console.WriteLine("Camera: Camera not running yet, or empty frame");

                if (_curFrame.Empty())
                {
                    Console.WriteLine("Received an empty frame.");
                }
                else
                    Console.WriteLine("Received frame.");
            }
            return _curFrame;
        }
        public void StartCamera()
        {
            _video = new VideoCapture(0);

            Task.Run(() => cameraThread());
        }

        public void StopCamera()
        {
            _cameraRunning = false;

        }
        public bool CheckCamera()
        {
            _video = new VideoCapture(0);
            bool result = default;
            int tries = 0;
            Console.WriteLine("Checking camera...");
            _video.Open(0);
            if (!_video.IsOpened())
            {
                Console.WriteLine("Cannot open camera, no image input available...");
                return false;
            }
            Mat tmp = new Mat();

            _video.Grab();
            tmp = _video.RetrieveMat();
            while (tmp.Empty() && tries++ < 5)
            {
                _video.Read(tmp);
            }
            if (tmp.Empty())
            {
                Console.WriteLine("No usable frame from camera after retries...");
            }
            else
            {
                Console.WriteLine("Camera OK...");
                result = true;
            }
            _video.Release();
            tmp.Release();
            return result;
        }

    }
}
