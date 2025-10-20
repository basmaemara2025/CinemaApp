using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.Areas.Admin.ViewModels
{
    public class MovieFormViewModel
    {
        public Movie Movie { get; set; } = new Movie();

        public IEnumerable<Category> Categories { get; set; }= new List<Category>();
        public IEnumerable<Cinema> Cinemas { get; set; } = new List<Cinema>();

    }
}
