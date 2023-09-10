using System.Diagnostics;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

[EnableCors]
public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;

    private readonly DataContext _context;

    public HomeController(ILogger<HomeController> logger, DataContext context,
        IDbInitializer initializer) {
        _logger = logger;
        _context = context;

        initializer.Initialize();
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}