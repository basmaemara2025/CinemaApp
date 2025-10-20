namespace CinemaApp.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MainImage { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public bool IsTopMovie { get; set; } = false;       
    public DateTime ReleaseDate { get; set; } = DateTime.UtcNow; 

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int CinemaId { get; set; }
    public Cinema? Cinema { get; set; }

    public ICollection<MovieSubImage> MovieSubImages { get; set; }= new List<MovieSubImage>();
    public ICollection<Booking> Bookings { get; set; }=new List<Booking>(); 
    public ICollection<Actor> Actors { get; set; } = new List<Actor>();
}
