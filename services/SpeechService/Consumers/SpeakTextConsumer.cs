using Contracts;
using MassTransit;
using SpeechService.Models;
using SpeechService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechService.Consumers
{
    internal class SpeakTextConsumer : IConsumer<SpeakText>
    {
        private readonly SpeechSvc speechSvc;

        public SpeakTextConsumer(SpeechSvc speechSvc)
        {
            this.speechSvc = speechSvc;
        }
        public async Task Consume(ConsumeContext<SpeakText> context)
        {
            while (speechSvc.IsSpeaking())
                await Task.Delay(100);
            speechSvc.Speak(new SpeakDTO(context.Message.Text));
        }
    }
}
