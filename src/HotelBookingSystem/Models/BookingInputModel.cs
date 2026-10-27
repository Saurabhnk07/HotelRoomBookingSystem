using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models;

public class BookingInputModel
{
    [Required]
    public int RoomId { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Full name")]
    public string GuestName { get; set; } = "";

    [Required, EmailAddress, StringLength(150)]
    public string GuestEmail { get; set; } = "";

    [Required, DataType(DataType.Date)]
    [Display(Name = "Check-in")]
    public DateTime CheckIn { get; set; } = DateTime.Today.AddDays(1);

    [Required, DataType(DataType.Date)]
    [Display(Name = "Check-out")]
    public DateTime CheckOut { get; set; } = DateTime.Today.AddDays(2);

    [Range(1, 10)]
    public int Guests { get; set; } = 1;
}
