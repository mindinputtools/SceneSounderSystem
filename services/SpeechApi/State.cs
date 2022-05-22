using ESpeakSynthSharp;
using SpeechApi.Models;
using System.Collections.Concurrent;

namespace SpeechApi
{
    public static class State
    {
        public static ConcurrentQueue<QueueEntry> SpeechQueue { get; set; } = new();
        public static ESpeakSynth CurrentSpeaker { get; private set; }
        public static Thread SpeakerThread { get; internal set; }

        public static Guid? SpeakingId = null;
        public static bool IsSpeaking = false;
        public static bool StopAll = false;
        public static bool QueueRunning = false;
        public static async void ProcessQueue()
        {
            if (SpeechQueue.IsEmpty) return;
            bool done = false;
            State.QueueRunning = true;


            do
            {


                if (SpeechQueue.TryDequeue(out var queueEntry))
                {
                    using (var speaker = new ESpeakSynth())
                    {
                        State.CurrentSpeaker = speaker;
                        var reset = new AutoResetEvent(false);
                        var completed = () =>
                        {
                            Console.WriteLine("Speech completed!");

                            reset.Set();
                            IsSpeaking = false;
                            SpeakingId = null;
                            return;
                        };

                        speaker.SetCompleted(completed);

                        speaker.Speak(queueEntry.Speak.Text);
                        IsSpeaking = true;
                        SpeakingId = queueEntry.Id;

                        Console.WriteLine("Waiting for speech to complete..");
                        reset.WaitOne(TimeSpan.FromSeconds(120));
                        Console.WriteLine("Waiting done!");
                        if (!string.IsNullOrEmpty(queueEntry.CallbackUrl))
                        {
                            using (var http = new HttpClient())
                            {
                                await http.GetAsync($"{queueEntry.CallbackUrl}?id={queueEntry.Id}");
                                Console.WriteLine($"Send speech completed to {queueEntry.CallbackUrl}");
                            }
                        }
                    }
                }
                else done = true;
            } while (!done);
            QueueRunning = false;
            CurrentSpeaker = null;
            SpeakerThread = null;
            Console.WriteLine("Finished processing speech queue");
        }
    }
}
