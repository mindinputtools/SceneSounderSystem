using MassTransit;
using SpeechService;
using SpeechService.Consumers;
using SpeechService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<SpeakTextConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddScoped<SpeechSvc>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
