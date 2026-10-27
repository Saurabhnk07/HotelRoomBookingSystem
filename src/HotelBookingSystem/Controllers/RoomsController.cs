using HotelBookingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Controllers;

public class RoomsController : Controller
{
    private readonly HotelBookingDbContext _db;
    public RoomsController(HotelBookingDbContext db) => _db = db;

    public async Task<IActionResult> Details(int id)
    {
        var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        return room == null ? NotFound() : View(room);
    }
}
