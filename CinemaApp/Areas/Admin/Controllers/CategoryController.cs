using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        ApplicationDbContext _DbContext = new();
        public IActionResult Index()
        {
            var categories = _DbContext.Categories.AsNoTracking().AsQueryable();
            return View(categories.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        { 
        
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
           // Category category = new() {Name=name,Status=status };
            _DbContext.Categories.Add(category);
            _DbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
           

        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var category = _DbContext.Categories.FirstOrDefault(e => e.Id == Id);
            if (category == null)
                return RedirectToAction("NotFoundPage", "Home");
            else
            {
                return View(category);
            }
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
           // Category category = new() {Name=name,Status=status };
            _DbContext.Categories.Update(category);
            _DbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
           

        }
        public IActionResult Delete(int Id)
        {
            var category = _DbContext.Categories.FirstOrDefault(e => e.Id == Id);
            if (category == null)
                return RedirectToAction("NotFoundPage", "Home");

            _DbContext.Categories.Remove(category);
            _DbContext.SaveChanges();
            return RedirectToAction(nameof(Index));


        }


    }
}
