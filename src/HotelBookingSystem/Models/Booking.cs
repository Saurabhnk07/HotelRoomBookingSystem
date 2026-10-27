using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystem.Models;

public enum BookingStatus { Pending = 0, Confirmed = 1, Cancelled = 2 }

public class Booking
{
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }
    public Room? Room { get; set; }

    [Required, StringLength(100)]
    public string GuestName { get; set; } = "";

    [Required, EmailAddress, StringLength(150)]
    public string GuestEmail { get; set; } = "";

    [Required, DataType(DataType.Date)]
    public DateTime CheckIn { get; set; }

    [Required, DataType(DataType.Date)]
    public DateTime CheckOut { get; set; }

    [Range(1, 10)]
    public int Guests { get; set; } = 1;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
