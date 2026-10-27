using HotelBookingSystem.Models;

namespace HotelBookingSystem.Services;

public interface IBookingService
{
    Task<bool> IsAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
    Task<(bool Success, string Message, Booking? Booking)> CreateAsync(BookingInputModel input);
}
