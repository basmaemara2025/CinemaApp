using CinemaApp.Models;
using CinemaApp.Repositories;
using ECommerce.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                includes: [e => e.Category],
                tracked: false
            );

            // filtering
            if (category is not null)
                movies = movies.Where(m => m.CategoryId == category);

            if (!string.IsNullOrWhiteSpace(moviename))
                movies = movies.Where(m => m.Title.Contains(moviename));

            // pagination
            int pageSize = 3;
            int totalCount = movies.Count();
            double totalPages = Math.Ceiling(totalCount / (double)pageSize);

            var pagedMovies = movies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // categories list
            var categories = await _categoryRepo.GetAsync();

            // ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.Categories = categories;
            ViewBag.MovieName = moviename;
            ViewBag.Category = category;
            ViewBag.CurrentPage = page;

            return View(pagedMovies);
        }
    }
}

