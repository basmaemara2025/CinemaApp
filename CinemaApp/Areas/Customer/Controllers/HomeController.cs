using Microsoft.AspNetCore.Mvc;
using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;
namespace CinemaApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _Dbcontext = new();

        public IActionResult Index(int? category, string moviename, int page = 1)
        {
            var movies = _Dbcontext.Movies.Include(s => s.Category).AsQueryable();
            var totalpages = Math.Ceiling(movies.Count() / 3.0);
            ViewBag.Totalpages = totalpages;
            movies = movies.Skip((page - 1) * 3).Take(3);

            var categories = _Dbcontext.Categories;
            ViewBag.categories = categories.AsEnumerable();
            if (category is not null)
            {
                movies = _Dbcontext.Movies.Where(m=> m.CategoryId == category);
            }
            if (moviename is not null)
            {
                movies = _Dbcontext.Movies.Where(m=> m.Title.Contains(moviename));
            }
            ViewBag.MovieName = moviename;
            ViewBag.category = category;
            ViewBag.currentpage = page;

            return View( movies.ToList());
        }
    }
}
