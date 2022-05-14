using SpeechClient;

namespace SystemApi.Services
{
    public class LifeCycleService : IHostedService
    {
        private readonly IConfiguration configuration;
        private readonly Speaker speaker;

        public LifeCycleService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            speaker = new Speaker(new Uri(configuration["SpeechApiAddress"]), httpClientFactory);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Delay(3000);
            speaker.SpeakText("SceneSounder started!");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            speaker.SpeakText("SceneSounder shutting down!");
        }
    }
}
