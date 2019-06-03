using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SoccerStandings.Models;
using SoccerStandings.Services;

namespace SoccerStandings.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices] IStandingsService standingsService)
        {
            var model = new StandingsModel(standingsService);
            model.CalculateStandings();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
