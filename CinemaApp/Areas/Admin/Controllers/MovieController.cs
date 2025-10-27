using CinemaApp.Areas.Admin.ViewModels;
using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {

        ApplicationDbContext _DbContext;
        public MovieController(ApplicationDbContext dbcontext)
        {
            _DbContext = dbcontext;
        }




        public IActionResult Index()
        {
            var movies = _DbContext.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .AsNoTracking()
                .ToList();

            return View(movies);
        }



        public IActionResult Create()
        {
            var viewModel = new MovieFormViewModel
            {
                Categories = _DbContext.Categories.ToList(),
                Cinemas = _DbContext.Cinemas.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        
        public IActionResult Create(MovieFormViewModel model, IFormFile MainImageFile, List<IFormFile>? SubImageFiles)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _DbContext.Categories.ToList();
                model.Cinemas = _DbContext.Cinemas.ToList();
                return View(model);
            }

            
            if (MainImageFile != null)
            {
                string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "assets", "MovieImages");
                Directory.CreateDirectory(mainFolder);

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(MainImageFile.FileName);
                string filePath = Path.Combine(mainFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    MainImageFile.CopyTo(stream);
                }

                model.Movie.MainImage = "/Admin/assets/MovieImages/" + uniqueFileName;
            }

            
            if (SubImageFiles != null && SubImageFiles.Any())
            {
                string subFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "assets", "MovieImages", "MovieSubImages");
                Directory.CreateDirectory(subFolder);

                foreach (var subFile in SubImageFiles)
                {
                    string subFileName = Guid.NewGuid() + Path.GetExtension(subFile.FileName);
                    string subFilePath = Path.Combine(subFolder, subFileName);

                    using (var stream = new FileStream(subFilePath, FileMode.Create))
                    {
                        subFile.CopyTo(stream);
                    }

                    model.Movie.MovieSubImages.Add(new MovieSubImage
                    {
                        ImagePath = "/Admin/assets/MovieImages/MovieSubImages/" + subFileName
                    });
                }
            }

            _DbContext.Movies.Add(model.Movie);
            _DbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _DbContext.Movies
                .Include(m => m.MovieSubImages)
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null) return NotFound();

            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Categories = _DbContext.Categories.ToList(),
                Cinemas = _DbContext.Cinemas.ToList()
            };

            return View(viewModel);
        }




        [HttpPost]
       
        public IActionResult Edit(MovieFormViewModel model, IFormFile? MainImageFile, List<IFormFile>? SubImageFiles)
        {
            var movieInDb = _DbContext.Movies
                .Include(m => m.MovieSubImages)
                .FirstOrDefault(m => m.Id == model.Movie.Id);

            if (movieInDb == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Categories = _DbContext.Categories.ToList();
                model.Cinemas = _DbContext.Cinemas.ToList();
                return View(model);
            }

            // 🟢 تحديث الخصائص الأساسية
            movieInDb.Title = model.Movie.Title;
            movieInDb.Description = model.Movie.Description;
            movieInDb.Price = model.Movie.Price;
            movieInDb.CategoryId = model.Movie.CategoryId;
            movieInDb.CinemaId = model.Movie.CinemaId;
            movieInDb.IsTopMovie = model.Movie.IsTopMovie;
            movieInDb.ReleaseDate = model.Movie.ReleaseDate;

            // 🟢 تحديث الصورة الرئيسية
            if (MainImageFile != null)
            {
                string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "assets", "MovieImages");
                Directory.CreateDirectory(mainFolder);

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(MainImageFile.FileName);
                string filePath = Path.Combine(mainFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    MainImageFile.CopyTo(stream);
                }

                // حذف الصورة القديمة (اختياري)
                if (!string.IsNullOrEmpty(movieInDb.MainImage))
                {
                    string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", movieInDb.MainImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                movieInDb.MainImage = "/Admin/assets/MovieImages/" + uniqueFileName;
            }

            // 🟣 إضافة الصور الفرعية الجديدة
            if (SubImageFiles != null && SubImageFiles.Any())
            {
                string subFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "assets", "MovieImages", "MovieSubImages");
                Directory.CreateDirectory(subFolder);

                foreach (var subFile in SubImageFiles)
                {
                    string subFileName = Guid.NewGuid() + Path.GetExtension(subFile.FileName);
                    string subFilePath = Path.Combine(subFolder, subFileName);

                    using (var stream = new FileStream(subFilePath, FileMode.Create))
                    {
                        subFile.CopyTo(stream);
                    }

                    movieInDb.MovieSubImages.Add(new MovieSubImage
                    {
                        ImagePath = "/Admin/assets/MovieImages/MovieSubImages/" + subFileName
                    });
                }
            }

            _DbContext.SaveChanges();
            return RedirectToAction("Index");
        }




        [HttpPost]
        public IActionResult Delete(int id)
        {
            var movie = _DbContext.Movies
                .Include(m => m.MovieSubImages)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            if (!string.IsNullOrEmpty(movie.MainImage))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", movie.MainImage.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            if (movie.MovieSubImages != null)
            {
                foreach (var sub in movie.MovieSubImages)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", sub.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
            }

            _DbContext.Movies.Remove(movie);
            _DbContext.SaveChanges();

            return Ok();
        }



        [HttpPost]
        public IActionResult DeleteSubImage(int id)
        {
            var subImage = _DbContext.MovieSubImages.FirstOrDefault(s => s.Id == id);
            if (subImage == null) return NotFound();

            
            if (!string.IsNullOrEmpty(subImage.ImagePath))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", subImage.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            }

            _DbContext.MovieSubImages.Remove(subImage);
            _DbContext.SaveChanges();

            return Ok(); 
        }

    }
}
