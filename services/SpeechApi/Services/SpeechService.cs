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

        public bool Speak(SpeakDTO speakDTO)
        {
            if (State.Speaker.IsSpeaking) return false;
            // ToDo, figure out a better way to make sure the speech speaks the new text and not the previous one
            // This is a hack for now..
            State.Speaker.Stop();
            State.Speaker.Terminate();
            State.Speaker = new ESpeakSynth();
            return State.Speaker.Speak(speakDTO.Text);
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
