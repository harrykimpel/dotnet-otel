using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Logs;

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
            .AddConsoleExporter()
            .AddOtlpExporter()
        )
    .WithMetrics(metricsProviderBuilder =>
        metricsProviderBuilder
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter()
            .AddMeter(DiagnosticsConfig.Meter.Name)
            .AddOtlpExporter()
        );

// Add OpenTelemetry Logs to our Service Collection
builder.Logging.AddOpenTelemetry(x =>
{
    x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"));
    x.IncludeFormattedMessage = true;
    x.IncludeScopes = true;
    x.ParseStateValues = true;
    x.AddOtlpExporter();
});

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
    public const string ServiceName = "MyService";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);

    public static Meter Meter = new(ServiceName);
    public static Counter<long> RequestCounter =
        Meter.CreateCounter<long>("app.request_counter");
}