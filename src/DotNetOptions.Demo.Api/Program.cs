using DotNetOptions.Demo.Api.BackgroundServices;
using DotNetOptions.Demo.Api.Interfaces;
using DotNetOptions.Demo.Api.Options;
using DotNetOptions.Demo.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(builder.Configuration.GetConnectionString("AppConfiguration"))
           .Select("Application:*", LabelFilter.Null)
           .ConfigureRefresh(refreshOptions =>
                refreshOptions.RegisterAll().SetRefreshInterval(TimeSpan.FromSeconds(1)));
           //.ConfigureRefresh(refreshOptions =>
                //refreshOptions.Register("Application:Sentinel", refreshAll: true).SetRefreshInterval(TimeSpan.FromSeconds(1)));
});

builder.Services.AddAzureAppConfiguration();

builder.Services.Configure<ApplicationOptions>(
    builder.Configuration.GetSection(ApplicationOptions.Name));

//Se quiser injetar ApplicationOptions diretamente, mas o reload não funcionará com IOptionsMonitor ou IOptionsSnapshot
//builder.Services.AddSingleton(sp =>
//    sp.GetRequiredService<IOptions<ApplicationOptions>>().Value
//);

builder.Services.AddScoped<IOptionsService, OptionsService>();
builder.Services.AddScoped<IOptionsSnapshotService, OptionsSnapshotService>();
builder.Services.AddScoped<IOptionsMonitorService, OptionsMonitorService>();

builder.Services.AddHostedService<OptionsMonitorBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAzureAppConfiguration();

app.UseHttpsRedirection();

app.MapGet("/", () => new
{
    message = "ASP.NET Core Options Pattern Demo",
    endpoints = new[]
    {
        "/options/options - IOptions (Singleton, não recarrega)",
        "/options/options-snapshot - IOptionsSnapshot (Scoped, recarrega por request)",
        "/options/options-monitor - IOptionsMonitor (Singleton, recarrega em tempo real)",
        "/options/compare - Comparar todos os tipos"
    },
    instructions = "Edite o arquivo appsettings.json para ver o comportamento de reload. " +
                   "Verifique os logs para ver o BackgroundService detectando mudanças."
});

// Endpoint para IOptions
app.MapGet("/options/options", ([FromServices] IOptionsService service) => new
{
    type = "IOptions<T>",
    description = service.GetDescription(),
    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
    options = service.GetOptions()
});

// Endpoint para IOptionsSnapshot
app.MapGet("/options/options-snapshot", ([FromServices] IOptionsSnapshotService service) => new
{
    type = "IOptionsSnapshot<T>",
    description = service.GetDescription(),
    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
    options = service.GetOptions()
});

// Endpoint para IOptionsMonitor
app.MapGet("/options/options-monitor", ([FromServices] IOptionsMonitorService service) => new
{
    type = "IOptionsMonitor<T>",
    description = service.GetDescription(),
    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
    options = service.GetOptions()
});

// Endpoint para comparar todos os tipos
app.MapGet("/options/compare", (
    [FromServices] IOptionsService optionsService,
    [FromServices] IOptionsSnapshotService snapshotService,
    [FromServices] IOptionsMonitorService monitorService) => new
    {
        message = "Comparação entre todos os tipos de Options Pattern",
        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
        ioptions = new
        {
            type = "IOptions<T>",
            description = optionsService.GetDescription(),
            values = optionsService.GetOptions()
        },
        ioptionsSnapshot = new
        {
            type = "IOptionsSnapshot<T>",
            description = snapshotService.GetDescription(),
            values = snapshotService.GetOptions()
        },
        ioptionsMonitor = new
        {
            type = "IOptionsMonitor<T>",
            description = monitorService.GetDescription(),
            values = monitorService.GetOptions()
        }
    });

app.Run();
