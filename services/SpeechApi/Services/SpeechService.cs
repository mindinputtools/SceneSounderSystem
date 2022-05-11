using ESpeakSynthSharp;
using MITAudioLib;
using SpeechApi.Models;

namespace SpeechApi.Services
{
    public class SpeechService
    {
        public SpeechService()
        {
            if (State.Speaker == null) State.Speaker = new ESpeakSynth();
        }

        public Guid Speak(SpeakDTO speakDTO)
        {
            QueueEntry queueEntry = new QueueEntry();
            queueEntry.Speak = speakDTO;
            State.SpeechQueue.Enqueue(queueEntry);
            if (!State.QueueRunning) Task.Run(() => State.ProcessQueue());
            return queueEntry.Id;
        }
        public bool IsSpeaking()
        {
            return State.Speaker.IsSpeaking;
        }
        public void Stop()
        {
            if (State.Speaker.IsSpeaking) State.Speaker.Stop();
        }
    }
}
