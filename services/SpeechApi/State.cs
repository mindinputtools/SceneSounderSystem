using ESpeakSynthSharp;
using SpeechApi.Models;
using System.Collections.Concurrent;

namespace SpeechApi
{
    public static class State
    {
        public static ConcurrentQueue<QueueEntry> SpeechQueue { get; set; } = new();
        public static Guid? SpeakingId = null;
        public static bool IsSpeaking = false;
        public static bool StopAll = false;
        public static bool QueueRunning = false;
        public static void ProcessQueue()
        {
            if (SpeechQueue.IsEmpty) return;
            bool done = false;
            State.QueueRunning = true;
                    

            do
            {

                
                if (SpeechQueue.TryDequeue(out var queueEntry))
                {
                    using (var speaker = new ESpeakSynth()) {
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
                }
                }
                else done = true;
            } while (!done);
            QueueRunning = false;

            Console.WriteLine("Finished processing speech queue");
        }
    }
}
