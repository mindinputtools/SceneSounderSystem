using ObjDetectYOLO.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<YOLOService>();
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


app.MapGet("/api/objdetect/yolo", async (YOLOService yOLO, bool? auto, bool? speak) =>
{
    var preds = await yOLO.YOLOPredictionsFromCamera();
    if (preds.Any()) preds = preds.OrderByDescending(o => o.Score);

    if (speak != null && speak.Value)
    {
        var speaker = new SpeechClient.Speaker();
        if (!preds.Any()) speaker.SpeakText("Didn't recognize any known objects!")
        foreach (var p in preds)
        {
            speaker.SpeakText($"{p.Label.Name}"); // , probability = {Math.Round(p.Score, 2)} percent
        }
    }
    return preds;
});


app.Run();

