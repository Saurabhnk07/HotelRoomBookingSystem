using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Services;

public class BookingService : IBookingService
{
    private readonly HotelBookingDbContext _db;
    public BookingService(HotelBookingDbContext db) => _db = db;

    public async Task<bool> IsAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        if (checkOut.Date <= checkIn.Date) return false;
        return !await _db.Bookings.AnyAsync(b =>
            b.RoomId == roomId &&
            b.Status != BookingStatus.Cancelled &&
            checkIn.Date < b.CheckOut.Date &&
            checkOut.Date > b.CheckIn.Date);
    }

    public async Task<(bool Success, string Message, Booking? Booking)> CreateAsync(BookingInputModel input)
    {
        var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == input.RoomId && r.IsActive);
        if (room == null) return (false, "This room is no longer available.", null);
        if (input.CheckIn.Date < DateTime.Today)
            return (false, "Check-in cannot be in the past.", null);
        if (input.CheckOut.Date <= input.CheckIn.Date)
            return (false, "Check-out must be after check-in.", null);
        if (input.Guests > room.Capacity)
            return (false, $"This room can accommodate up to {room.Capacity} guests.", null);
        if (!await IsAvailableAsync(room.Id, input.CheckIn, input.CheckOut))
            return (false, "This room is already booked for some or all of those dates.", null);

        var nights = (input.CheckOut.Date - input.CheckIn.Date).Days;
        var booking = new Booking
        {
            RoomId = room.Id,
            GuestName = input.GuestName.Trim(),
            GuestEmail = input.GuestEmail.Trim(),
            CheckIn = input.CheckIn.Date,
            CheckOut = input.CheckOut.Date,
            Guests = input.Guests,
            TotalPrice = nights * room.PricePerNight,
            Status = BookingStatus.Confirmed
        };
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();
        booking.Room = room;
        return (true, "Your booking has been confirmed.", booking);
    }
}
