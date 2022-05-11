using SpeechApi.Models;
using SpeechApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<SpeechService>();
builder.Services.AddHostedService<LifeCycleService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapPost("/api/speech", async (SpeechService speech, SpeakDTO speak) =>
{
    return await speech.Speak(speak);
});
app.MapGet("/speech", (SpeechService speech, bool? stop) =>
{
    if (stop != null && stop.Value) speech.Stop();
    return speech.IsSpeaking();
});


app.Run();

