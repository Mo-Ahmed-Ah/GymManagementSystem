using GymManagementSystem.BLL.Services.AnalyticsServices.Interfaces;
using GymManagementSystem.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagementSystem.MVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAnalyticsServices _analyticsServices;
    public HomeController(ILogger<HomeController> logger, IAnalyticsServices analyticsServices)
    {

        _logger = logger;
        _analyticsServices = analyticsServices;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("User : {UserName} Start To Get Data To Display It In Analytics Page This User Is At {startTime}", User.Identity!.Name, DateTime.Now);
        var result = await _analyticsServices.GetAnalyticsAsync();
        _logger.LogInformation("Get Data To Display In Analytics Page Is Finshed In {FinshedTime}", DateTime.Now);
        return View(result.Value);
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
