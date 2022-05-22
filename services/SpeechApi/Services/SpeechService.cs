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
            if (!string.IsNullOrEmpty(speakDTO.CallbackUrl)) queueEntry.CallbackUrl = speakDTO.CallbackUrl;
            State.SpeechQueue.Enqueue(queueEntry);
            if (!State.QueueRunning)
            {
                State.SpeakerThread = new Thread(State.ProcessQueue);
                State.SpeakerThread.Start();
            }
            return queueEntry.Id;
        }
        public bool IsSpeaking()
        {
            return State.IsSpeaking;
        }
        public void Stop()
        {
            if (State.IsSpeaking && State.CurrentSpeaker != null) State.CurrentSpeaker.Stop();
        }
    }
}
