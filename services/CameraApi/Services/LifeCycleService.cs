namespace CameraApi.Services
{
    public class LifeCycleService : IHostedService
    {
        private readonly CameraService cs;

        public LifeCycleService(CameraService cs)
        {
            this.cs = cs;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cs.Running())
            {
                Console.WriteLine("Stopping camera...");
                await cs.Stop();
            }
            
        }
    }
}
