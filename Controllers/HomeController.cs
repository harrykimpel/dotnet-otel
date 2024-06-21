using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnet_otel.Models;

namespace dotnet_otel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        //DiagnosticsConfig.logger.LogInformation(eventId: 123, "Starting some work in the Index controller ...");
        Console.WriteLine("Starting some work in the Index controller ...");

        //DiagnosticsConfig.logger.LogInformation(eventId: 123, "setting activity tags");
        Console.WriteLine("setting activity tags");
        // Track work inside of the request
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("SayHello");
        // activity?.SetTag("foo", 1);
        // activity?.SetTag("bar", "Hello, World!");
        // activity?.SetTag("baz", new int[] { 1, 2, 3 });

        //DiagnosticsConfig.logger.LogInformation(eventId: 123, "updating the metrics counter");
        Console.WriteLine("updating the metrics counter");
        // DiagnosticsConfig.RequestCounter.Add(1,
        //     new("Action", nameof(Index)),
        //     new("Controller", nameof(HomeController)));

        //DiagnosticsConfig.logger.LogInformation(eventId: 123, "showing the view");
        Console.WriteLine("showing the view");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
