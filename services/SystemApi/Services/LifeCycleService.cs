using SpeechClient;

namespace SystemApi.Services
{
    public class LifeCycleService : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly Speaker speaker;

        public LifeCycleService(IConfiguration configuration)
        {
            this.configuration = configuration;
            speaker = new Speaker();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var result = await speaker.SpeakText("SceneSounder started!");
            while (!stoppingToken.IsCancellationRequested)
            {
                result = await speaker.SpeakText("I'm checking my state..");
                await Task.Delay(5000, stoppingToken);
            }
            result = await speaker.SpeakText("SceneSounder shutting down!");
        }
    }
}
