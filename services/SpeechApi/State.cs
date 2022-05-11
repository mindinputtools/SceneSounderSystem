using ESpeakSynthSharp;
using SpeechApi.Models;
using System.Collections.Concurrent;

namespace SpeechApi
{
    public static class State
    {
        public static ConcurrentQueue<QueueEntry> SpeechQueue { get; set; } = new();
        public static ESpeakSynth Speaker = null;
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
                    var reset = new AutoResetEvent(false);
                    var completed = () =>
                    {
                        Console.WriteLine("Speech completed!");
                        reset.Set();
                    };
                    Speaker = new ESpeakSynth(completed);
                    Speaker.Speak(queueEntry.Speak.Text);
                    Console.WriteLine("Waiting for speech to complete..");
                    reset.WaitOne(TimeSpan.FromSeconds(120));
                    Console.WriteLine("Waiting done!");
                    Speaker.Terminate();
                }
                else done = true;
            } while (!done);
            QueueRunning = false;
            Console.WriteLine("Finished processing speech queue");
        }
    }
}
