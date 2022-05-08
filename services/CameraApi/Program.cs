using CameraApi.Interfaces;
using CameraApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICamera, OpenCVCamera>();
builder.Services.AddScoped<CameraService>();
var app = builder.Build();

// Configure the HTTP request pipeline.


app.MapGet("/api/camera/check", (CameraService cs) =>
{
    return cs.Check();
});

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}