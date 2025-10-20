﻿namespace CinemaApp.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
   // public string Password { get; set; } = string.Empty;
   // public bool IsAdmin { get; set; }

    public ICollection<Booking> Bookings { get; set; }=new List<Booking>();
}
