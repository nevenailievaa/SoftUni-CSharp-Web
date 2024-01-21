using Microsoft.AspNetCore.Mvc;
using MVCIntroDemo.Models;
using System.Diagnostics;

namespace MVCIntroDemo.Controllers;

public class HomeController : Controller
{
    //Fields
    private readonly ILogger<HomeController> logger;

    //Constructor
    public HomeController(ILogger<HomeController> logger)
    {
        logger = logger;
    }

    //Methods
    public IActionResult Index()
    {
        ViewBag.Message = "Hello World!";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewBag.Message = "This is an ASP.NET Core MVC app.";
        return View();
    }

    public IActionResult Numbers()
    {
        return View();
    }

    public IActionResult NumbersToN(int count = 3)
    {
        ViewBag.Count = count;
        return View();
    }

    //Error
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}