using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    
    {
        ApplicationDbContext _DbContext;
        public CinemaController(ApplicationDbContext dbcontext)
        {
            _DbContext = dbcontext;
        }

        
        public IActionResult Index()
        {
            var cinemas = _DbContext.Cinemas.AsNoTracking().AsQueryable();
            return View(cinemas.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema)
        {
            // Category category = new() {Name=name,Status=status };
            _DbContext.Cinemas.Add(cinema);
            _DbContext.SaveChanges();
            return RedirectToAction(nameof(Index));


        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var cinema = _DbContext.Cinemas.FirstOrDefault(e => e.Id == Id);
            if (cinema == null)
                return RedirectToAction("NotFoundPage", "Home");
            else
            {
                return View(cinema);
            }
        }
        [HttpPost]
        public IActionResult Edit(Cinema cinema)
        {
            // Category category = new() {Name=name,Status=status };
            _DbContext.Cinemas.Update(cinema);
            _DbContext.SaveChanges();
            return RedirectToAction(nameof(Index));


        }
        public IActionResult Delete(int Id)
        {
            var cinema = _DbContext.Cinemas.FirstOrDefault(e => e.Id == Id);
            if (cinema == null)
                return RedirectToAction("NotFoundPage", "Home");

            _DbContext.Cinemas.Remove(cinema);
            _DbContext.SaveChanges();
            return RedirectToAction(nameof(Index));


        }


    }
}
