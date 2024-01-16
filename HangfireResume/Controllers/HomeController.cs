using System.Diagnostics;
using System.Linq.Expressions;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using HangfireResume.Models;

namespace HangfireResume.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        BackgroundJob.Enqueue(() => doThis());
        return View();
    }

    public static void doThis()
    {
        var i = 10;
        while (i > 0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
            --i;
        }
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