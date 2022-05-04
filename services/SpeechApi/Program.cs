using SpeechApi.Models;
using SpeechApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<SpeechService>();
builder.Services.AddHostedService<SpeechService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapPost("/speech", (SpeechService speech, SpeakDTO speak) =>
{
    return speech.Speak(speak);
});
app.MapGet("/speech", (SpeechService speech) =>
{
    return speech.IsSpeaking();
});


app.Run();

