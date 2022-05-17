using CameraApi.Interfaces;
using CameraApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICamera, OpenCVCamera>();
builder.Services.AddScoped<CameraService>();
builder.Services.AddHostedService<CameraApi.Services.LifeCycleService>();
var app = builder.Build();

// Configure the HTTP request pipeline.


app.MapGet("/api/camera/check", async (CameraService cs) =>
{
    return await cs.Check();
});
app.MapGet("/api/camera/running", (CameraService cs) =>
{
    return cs.Running();
});
app.MapGet("/api/camera/start", async (CameraService cs) =>
{
    await cs.Start();
});
app.MapGet("/api/camera/stop", async (CameraService cs) =>
{
    await cs.Stop();
});
app.MapGet("/api/camera/image", async (CameraService cs) =>
{
    return await cs.GetImage();
});

app.Run();

