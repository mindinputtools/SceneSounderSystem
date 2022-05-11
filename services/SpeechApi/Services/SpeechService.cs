using ESpeakSynthSharp;
using MITAudioLib;
using SpeechApi.Models;

namespace SpeechApi.Services
{
    public class SpeechService
    {
        public SpeechService()
        {
        }

        public async Task<Guid> Speak(SpeakDTO speakDTO)
        {
            QueueEntry queueEntry = new QueueEntry();
            queueEntry.Speak = speakDTO;
            State.SpeechQueue.Enqueue(queueEntry);
            if (!State.QueueRunning) await Task.Run(State.ProcessQueue);
            return queueEntry.Id;
        }
        public bool IsSpeaking()
        {
            return State.IsSpeaking;
        }
        public void Stop()
        {
            if (State.IsSpeaking) State.StopAll = true;
        }
    }
}
