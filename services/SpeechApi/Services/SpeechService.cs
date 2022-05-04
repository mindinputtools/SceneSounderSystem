using ESpeakSynthSharp;
using MITAudioLib;
using SpeechApi.Models;

namespace SpeechApi.Services
{
    public class SpeechService : IHostedService
    {
        public SpeechService()
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            MITAudio.OpenAudio();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            MITAudio.CloseAudio();
            return Task.CompletedTask;
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
