using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSubImage> MovieSubImages { get; set; }
        
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=CinemaAppDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // العلاقة بين Booking و User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade); // حذف المستخدم يحذف حجوزاته

            // العلاقة بين Booking و Movie
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Movie)
                .WithMany()
                .HasForeignKey(b => b.MovieId)
                .OnDelete(DeleteBehavior.NoAction); // 🔥 مهم جدًا

            // العلاقة بين Booking و Cinema
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Cinema)
                .WithMany()
                .HasForeignKey(b => b.CinemaId)
                .OnDelete(DeleteBehavior.NoAction); // 🔥 مهم جدًا
        }



    }
}
