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

        public bool Speak(SpeakDTO speakDTO)
        {
            return ESpeakSynth.Speak(speakDTO.Text);
        }
        public bool IsSpeaking()
        {
            return ESpeakSynth.IsSpeaking;
        }
        public void Stop()
        {
//            if (IsSpeaking()) ESpeakSynth.Stop();
        }
    }
}
