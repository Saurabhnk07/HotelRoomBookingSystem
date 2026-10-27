using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly HotelBookingDbContext _db;
    public AdminController(HotelBookingDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Bookings.Include(b => b.Room).OrderByDescending(b => b.CreatedAtUtc).ToListAsync());

    public async Task<IActionResult> Rooms() => View(await _db.Rooms.OrderBy(r => r.Name).ToListAsync());

    [HttpGet]
    public IActionResult CreateRoom() => View(new Room());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoom(Room room)
    {
        if (!ModelState.IsValid) return View(room);
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Rooms));
    }

    [HttpGet]
    public async Task<IActionResult> EditRoom(int id)
    {
        var room = await _db.Rooms.FindAsync(id);
        return room == null ? NotFound() : View(room);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoom(Room room)
    {
        if (!ModelState.IsValid) return View(room);
        _db.Rooms.Update(room);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Rooms));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, BookingStatus status)
    {
        var booking = await _db.Bookings.FindAsync(id);
        if (booking == null) return NotFound();
        booking.Status = status;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
