using HotelBookingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Controllers;

public class HomeController : Controller
{
    private readonly HotelBookingDbContext _db;
    public HomeController(HotelBookingDbContext db) => _db = db;

    public async Task<IActionResult> Index(DateTime? checkIn, DateTime? checkOut, int guests = 1)
    {
        var start = checkIn?.Date ?? DateTime.Today.AddDays(1);
        var end = checkOut?.Date ?? start.AddDays(1);
        ViewBag.CheckIn = start.ToString("yyyy-MM-dd");
        ViewBag.CheckOut = end.ToString("yyyy-MM-dd");
        ViewBag.Guests = guests;

        var rooms = _db.Rooms.Where(r => r.IsActive && r.Capacity >= guests);
        if (end > start)
            rooms = rooms.Where(r => !r.Bookings.Any(b =>
                b.Status != Models.BookingStatus.Cancelled &&
                start < b.CheckOut && end > b.CheckIn));
        return View(await rooms.OrderBy(r => r.PricePerNight).ToListAsync());
    }

    public IActionResult Error() => View();
}
