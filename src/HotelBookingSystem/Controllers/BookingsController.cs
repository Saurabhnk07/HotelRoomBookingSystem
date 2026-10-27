using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using HotelBookingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Controllers;

public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly HotelBookingDbContext _db;

    public BookingsController(IBookingService bookingService, HotelBookingDbContext db)
    {
        _bookingService = bookingService;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int roomId, DateTime? checkIn, DateTime? checkOut, int guests = 1)
    {
        var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == roomId && r.IsActive);
        if (room == null) return NotFound();
        ViewBag.Room = room;
        return View(new BookingInputModel
        {
            RoomId = roomId,
            CheckIn = checkIn?.Date ?? DateTime.Today.AddDays(1),
            CheckOut = checkOut?.Date ?? DateTime.Today.AddDays(2),
            Guests = guests
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingInputModel input)
    {
        var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == input.RoomId && r.IsActive);
        if (room == null) return NotFound();
        ViewBag.Room = room;
        if (!ModelState.IsValid) return View(input);

        var result = await _bookingService.CreateAsync(input);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(input);
        }
        return RedirectToAction(nameof(Confirmation), new { id = result.Booking!.Id });
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var booking = await _db.Bookings.Include(b => b.Room).FirstOrDefaultAsync(b => b.Id == id);
        return booking == null ? NotFound() : View(booking);
    }

    [HttpGet]
    public IActionResult Find() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Find(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            ModelState.AddModelError(nameof(email), "Enter the email used for the booking.");
            return View();
        }
        ViewBag.Email = email.Trim();
        var bookings = await _db.Bookings.Include(b => b.Room)
            .Where(b => b.GuestEmail == email.Trim())
            .OrderByDescending(b => b.CreatedAtUtc).ToListAsync();
        return View("MyBookings", bookings);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, string email)
    {
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.GuestEmail == email);
        if (booking == null) return NotFound();
        if (booking.Status != BookingStatus.Cancelled)
        {
            booking.Status = BookingStatus.Cancelled;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Find));
    }
}
