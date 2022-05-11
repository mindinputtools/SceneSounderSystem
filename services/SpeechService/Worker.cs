using Contracts;
using MassTransit;
using MITAudioLib;

namespace SpeechService;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBus bus;

    public Worker(ILogger<Worker> logger, IBus bus)
    {
        _logger = logger;
        this.bus = bus;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        MITAudio.OpenAudio();
        await bus.Publish<ServiceState>(new { ServiceName = "SpeechService", Up = true }, cancellationToken);
//        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        MITAudio.CloseAudio();
        return Task.CompletedTask;
    }

}
