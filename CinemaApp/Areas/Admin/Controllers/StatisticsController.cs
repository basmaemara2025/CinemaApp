using CinemaApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticsController : Controller
    {
        ApplicationDbContext _DbContext;
        public StatisticsController(ApplicationDbContext dbcontext)
        {
            _DbContext = dbcontext;
        }

        public IActionResult Index()
        {
            ViewBag.CinemaCount = _DbContext.Cinemas.Count();
            ViewBag.MovieCount = _DbContext.Movies.Count();
            ViewBag.ActorCount = _DbContext.Actors.Count();
            ViewBag.CategoryCount = _DbContext.Categories.Count();

            // بيانات الرسم البياني
            var chartData = _DbContext.Cinemas
            .Select(c => new
            {
                c.Name,
                MovieCount = _DbContext.Movies.Count(m => m.CinemaId == c.Id)
            })
            .ToList();

            ViewBag.ChartLabels = string.Join(",", chartData.Select(c => $"'{c.Name}'"));
            ViewBag.ChartValues = string.Join(",", chartData.Select(c => c.MovieCount));

            return View();
        }
    }
}
