namespace ObjDetectYOLO.Services
{
    public class YoloLifeCycle : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            State.AutoSpeakerRunning = false;
        }
    }
}
