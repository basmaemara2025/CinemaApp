namespace CinemaApp.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int TicketsCount { get; set; }

        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }

        // 🔗 العلاقة مع المستخدم
        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
