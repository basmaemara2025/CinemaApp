namespace CinemaApp.Models;

public class Cinema
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public ICollection<Movie> Movies { get; set; }=new List<Movie>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
