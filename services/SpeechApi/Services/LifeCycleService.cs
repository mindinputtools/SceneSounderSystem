using MITAudioLib;

namespace SpeechApi.Services
{
    public class LifeCycleService : IHostedService
    {
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

    }
}
