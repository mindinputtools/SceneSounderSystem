using SpeechClient;

namespace SystemApi.Services
{
    public class SystemLifeCycle : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly Speaker speaker;

        public SystemLifeCycle(IConfiguration configuration)
        {
            this.configuration = configuration;
            speaker = new Speaker();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var result = await speaker.SpeakText("SceneSounder started!");
            Console.WriteLine($"Got Id {result}");
            while (!stoppingToken.IsCancellationRequested)
            {
                result = await speaker.SpeakText("I'm checking my state..");
                await Task.Delay(10000, stoppingToken);
            }
            result = await speaker.SpeakText("SceneSounder shutting down!");
        }
    }
}
