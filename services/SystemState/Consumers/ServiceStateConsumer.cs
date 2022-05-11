using Contracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemState.Consumers
{
    public class ServiceStateConsumer : IConsumer<ServiceState>
    {
        private readonly ILogger<ServiceStateConsumer> logger;

        public ServiceStateConsumer(ILogger<ServiceStateConsumer> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<ServiceState> context)
        {
            if (context.Message.Up)
                logger.LogInformation($"{context.Message.ServiceName} started");
        }
    }
}
