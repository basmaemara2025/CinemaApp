using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        ApplicationDbContext _DbContext = new();
        public IActionResult Index()
        {
            var actors = _DbContext.Actors.AsNoTracking().AsQueryable();
            return View(actors.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Actor actor, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "assets", "ActorImages");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    actor.ActorImage = fileName;
                }

                _DbContext.Actors.Add(actor);
                _DbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(actor);
        }

        // 🟢 GET: Admin/Actor/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var actor = _DbContext.Actors.Find(id);
            if (actor == null) return NotFound();

            return View(actor);
        }

        // 🟢 POST: Admin/Actor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Actor actor, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existingActor = _DbContext.Actors.Find(actor.Id);
                if (existingActor == null) return NotFound();

                existingActor.FullName = actor.FullName;

                if (ImageFile != null)
                {
                    // حذف الصورة القديمة لو موجودة
                    if (!string.IsNullOrEmpty(existingActor.ActorImage))
                    {
                        string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "assets", "ActorImages", existingActor.ActorImage);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    // رفع الصورة الجديدة
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "assets", "ActorImages");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    existingActor.ActorImage = fileName;
                }

                _DbContext.Actors.Update(existingActor);
                _DbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(actor);
        }

        // 🟢 GET: Admin/Actor/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var actor = _DbContext.Actors.Find(id);
            if (actor == null) return NotFound();

            // حذف الصورة من wwwroot/Admin/assets/ActorImages
            if (!string.IsNullOrEmpty(actor.ActorImage))
            {
                string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "assets", "ActorImages", actor.ActorImage);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            _DbContext.Actors.Remove(actor);
            _DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
