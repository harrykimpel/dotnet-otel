using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Logs;
using OpenTelemetry.Exporter;
using OpenTelemetry;

// DiagnosticsConfig.logger.LogInformation(eventId: 123, "Getting started!");
Console.WriteLine("Getting started!");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add OpenTelemetry Traces and Metrics to our Service Collection
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(DiagnosticsConfig.ActivitySource.Name)
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            //.AddConsoleExporter()
            .AddOtlpExporter()
        )
    .WithMetrics(metricsProviderBuilder =>
        metricsProviderBuilder
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            //.AddConsoleExporter()
            .AddMeter(DiagnosticsConfig.Meter.Name)
            .AddOtlpExporter()
        );

// Add OpenTelemetry Logs to our Service Collection
builder.Logging.AddOpenTelemetry(x =>
{
    x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(DiagnosticsConfig.ServiceName));
    x.IncludeFormattedMessage = true;
    x.IncludeScopes = true;
    x.ParseStateValues = true;
    x.AddOtlpExporter();
});

Console.WriteLine("Done with Otel config ...");
// DiagnosticsConfig.logger.LogInformation(eventId: 123, "Done with Otel config ...");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public static class DiagnosticsConfig
{
    public const string ServiceName = "dotnet-otel";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);

    public static Meter Meter = new(ServiceName);
    public static Counter<long> RequestCounter =
        Meter.CreateCounter<long>("app.request_counter");

    // public static ILogger logger = LoggerFactory.Create(builder =>
    //     {
    //         builder.AddOpenTelemetry(options =>
    //         {
    //             //options.AddConsoleExporter();
    //             options.AddOtlpExporter();
    //             options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
    //                 serviceName: ServiceName,
    //                 serviceVersion: "1.0.0"));
    //         });
    //     }).CreateLogger<Program>();
}
