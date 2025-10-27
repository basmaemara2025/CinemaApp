using CinemaApp.Models;
using CinemaApp.Repositories;
using ECommerce.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CinemaApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IRepository<Movie> _movieRepo;
        private readonly IRepository<Category> _categoryRepo;

        public HomeController(IRepository<Movie> movieRepo, IRepository<Category> categoryRepo)
        {
            _movieRepo = movieRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index(int? category, string? moviename, int page = 1)
        {
            // bring all movies with their categories
            var movies = await _movieRepo.GetAsync(
                includes: new Expression<Func<Movie, object>>[] { m => m.Category },
                tracked: false
            );

            // filtering
            if (category is not null)
                movies = movies.Where(m => m.CategoryId == category);

            if (!string.IsNullOrWhiteSpace(moviename))
                movies = movies.Where(m => m.Title.Contains(moviename));

            // pagination
            int pageSize = 3;
            var totalPages = Math.Ceiling(movies.Count() / (double)pageSize);
            var pagedMovies = movies.Skip((page - 1) * pageSize).Take(pageSize);

            // categories list
            var categories = await _categoryRepo.GetAsync();

            // ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.Categories = categories;
            ViewBag.MovieName = moviename;
            ViewBag.Category = category;
            ViewBag.CurrentPage = page;

            return View(pagedMovies.ToList());
        }
    }
}
