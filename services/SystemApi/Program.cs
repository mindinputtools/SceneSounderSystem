using SystemApi.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddScoped<SystemApi.Services.SystemService>();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<LifeCycleService>();
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


app.MapGet("/api/system/poweroff", (SystemApi.Services.SystemService systemService) =>
{
    systemService.Poweroff();
    return true;
});

app.Run();

