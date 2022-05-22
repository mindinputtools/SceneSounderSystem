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
        public async Task StartAutoSpeaker()
        {
            State.AutoSpeakerThread = new Thread(AutoSpeakThread);
            State.AutoSpeakerThread.Start();
        }
        public async Task StopAutoSpeaker()
        {
            State.AutoSpeakerRunning = false;
        }
        private async void AutoSpeakThread(object state)
        {
            var camera = new CameraClient.Camera();
            var speaker = new SpeechClient.Speaker();
            List<YoloPrediction>? preds = new List<YoloPrediction>();
            using var scorer = new YoloScorer<YoloCocoP5Model>(modelPath);
            State.AutoSpeakerRunning = true;
            await speaker.SpeakText("YOLO Autospeaker enabled.");
            while (State.AutoSpeakerRunning)
            {
                using (var stream = await camera.GetImageStreamAsync())
                {
                    using (var image = Image.FromStream(stream))
                    {
                        preds = scorer.Predict(image);
                        //                        if (preds.Any()) preds = preds.OrderByDescending(o => o.Score);
                        var textToSpeak = string.Empty;
                        if (!preds.Any())
                        {
                            speaker.SpeakText("Didn't recognize any known objects!", "http://objdetect-yolo:8080/api/objdetect/yolo/speechdone");
                            State.SpeechCompleted.WaitOne(TimeSpan.FromSeconds(5));
                        }
                        foreach (var p in preds)
                        {
                            textToSpeak += $"{p.Label.Name}. ";
                        }
                        if (!string.IsNullOrEmpty(textToSpeak))
                        {
                            speaker.SpeakText(textToSpeak, "http://objdetect-yolo:8080/api/objdetect/yolo/speechdone");
                            State.SpeechCompleted.WaitOne(TimeSpan.FromSeconds(5));
                        }
                    } // image
                } // camera
            }
            await speaker.SpeakText("YOLO Autospeaker stopped.");
            camera.Stop();
        }
    }
}
