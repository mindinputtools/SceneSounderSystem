using ObjDetectYOLO;
using ObjDetectYOLO.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<YOLOService>();
builder.Services.AddHostedService<YoloLifeCycle>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/api/objdetect/yolo", async (YOLOService yOLO, bool? speak) =>
{
    var preds = await yOLO.YOLOPredictionsFromCamera();
    if (preds.Any()) preds = preds.OrderByDescending(o => o.Score);

    if (speak != null && speak.Value)
    {
        var speaker = new SpeechClient.Speaker();
        if (!preds.Any()) speaker.SpeakText("Didn't recognize any known objects!");
        var textToSpeak = string.Empty;
        foreach (var p in preds)
        {
            textToSpeak += $"{p.Label.Name}. "; // , probability = {Math.Round(p.Score, 2)} percent
        }
        if (!string.IsNullOrEmpty(textToSpeak)) speaker.SpeakText(textToSpeak);
    }
    return preds;
});
app.MapGet("/api/objdetect/yolo/start", async (YOLOService yOLO) =>
{
    if (!State.AutoSpeakerRunning) await yOLO.StartAutoSpeaker();
});
app.MapGet("/api/objdetect/yolo/stop", async (YOLOService yOLO) =>
{
    if (State.AutoSpeakerRunning) await yOLO.StopAutoSpeaker();
});
app.MapGet("/api/objdetect/yolo/speechdone", () =>
{
    State.SpeechCompleted.Set();
});

app.Run();

