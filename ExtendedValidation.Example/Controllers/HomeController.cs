using System.Diagnostics;
using ExtendedValidation.Example.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ExtendedValidation.Example.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpPost("/")]
    public IActionResult Index([FromBody]ExampleRequest request)
    {
        return Json(request);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}