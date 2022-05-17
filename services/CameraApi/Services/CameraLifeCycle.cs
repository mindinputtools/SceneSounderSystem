namespace CameraApi.Services
{
    public class CameraLifeCycle : IHostedService
    {
        private readonly CameraService cs;

        public CameraLifeCycle(CameraService cs)
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
