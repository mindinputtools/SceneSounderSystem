using SystemApi.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddScoped<SystemApi.Services.SysService>();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<SystemLifeCycle>();
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


app.MapGet("/api/system/poweroff", async (SystemApi.Services.SysService systemService) =>
{
    await systemService.Poweroff();
    return true;
});
app.MapGet("/api/system/reboot", async (SystemApi.Services.SysService systemService) =>
{
    await systemService.Reboot();
    return true;
});
app.MapPost("/api/system/setaudiovolume", async (SystemApi.Services.SysService systemService, int vol) =>
{
    await systemService.SetAudioVolume(vol);
    return true;
});

app.Run();

