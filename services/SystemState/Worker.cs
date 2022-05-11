using Contracts;
using MassTransit;

namespace SystemState;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBus bus;

    public Worker(ILogger<Worker> logger, IBus bus)
    {
        _logger = logger;
        this.bus = bus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        bus.Publish<SpeakText>(new { Text = "Welcome to SceneSounder!" });
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        bus.Publish<SpeakText>(new { Text = "Shuting down SceneSounder!" });
        return Task.CompletedTask;
    }
}
