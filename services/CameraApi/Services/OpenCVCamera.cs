using CameraApi.Interfaces;
using OpenCvSharp;

namespace CameraApi.Services
{
    public class OpenCVCamera : ICamera
    {
        private VideoCapture _video;
        private Thread cameraThread;
        private bool _cameraRunning;
        private bool _saveFrame;
        private bool _gotFrame;
        private Action<Mat> GotFrame;

        public bool Running => _cameraRunning;

        public OpenCVCamera()
        {
            //            Console.WriteLine("Opening camera...");

        }
        private void cameraThreadProc()
        {
            _cameraRunning = true;
            while (_cameraRunning)
            {
                _video.Grab();
                Task.Delay(200);
            }
            _video.Release();
        }
        public async Task<Mat> GetMatImage()
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
        public async Task StartCamera()
        {
            _video = new VideoCapture(0);
            _video.Open(0);
            if (!_video.IsOpened())
            {
                Console.WriteLine("Cannot open camera, no image input available...");
                return;
            }
            Console.WriteLine($"Opened camera with resolution: {_video.FrameWidth}X{_video.FrameHeight}");
            cameraThread = new Thread(cameraThreadProc);
            cameraThread.Start();
        }

        public async Task StopCamera()
        {
            _cameraRunning = false;

        }
        public async Task<bool> CheckCamera()
        {
            if (_cameraRunning)
            {
                Console.WriteLine("Checking running camera...");
                var mat = await GetMatImage();
                if (!mat.Empty()) return true;
                Console.WriteLine("Camera NOT OK");
                return false;
            }
            else
            {
                _video = new VideoCapture(0);
                bool result = default;
                int tries = 0;
                Console.WriteLine("Checking non-running camera...");
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
}
