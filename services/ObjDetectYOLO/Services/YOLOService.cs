using System.Drawing;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;

namespace ObjDetectYOLO.Services
{
    public class YOLOService
    {
        private string modelPath = "Assets/Weights/yolov5s.onnx";

        public async Task<IEnumerable<YoloPrediction>> YOLOPredictionsFromCamera()
        {
            var camera = new CameraClient.Camera();
            var predictions = new List<YoloPrediction>();
            using var stream = await camera.GetImageStreamAsync();
            using var image = Image.FromStream(stream);
            using var scorer = new YoloScorer<YoloCocoP5Model>(modelPath);
            predictions = scorer.Predict(image);
            return predictions;
        }
    }
}
