using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystem.Models;

public class Room
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = "";

    [Required, StringLength(30)]
    public string RoomType { get; set; } = "Standard";

    [Range(1, 10)]
    public int Capacity { get; set; } = 2;

    [Column(TypeName = "decimal(10,2)"), Range(1, 100000)]
    public decimal PricePerNight { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = "";

    [StringLength(500)]
    public string ImageUrl { get; set; } = "";

    public bool IsActive { get; set; } = true;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
