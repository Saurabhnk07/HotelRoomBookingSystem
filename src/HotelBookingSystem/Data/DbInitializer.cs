using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<HotelBookingDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (!await db.Rooms.AnyAsync())
        {
            db.Rooms.AddRange(
                new Room { Name = "Deluxe King 101", RoomType = "Deluxe", Capacity = 2, PricePerNight = 3500, Description = "Comfortable king bed, work desk, Wi-Fi and ensuite bathroom.", ImageUrl = "https://images.unsplash.com/photo-1611892440504-42a792e24d32?auto=format&fit=crop&w=1000&q=80" },
                new Room { Name = "Family Suite 201", RoomType = "Suite", Capacity = 4, PricePerNight = 6200, Description = "Spacious suite for families with a sitting area and city views.", ImageUrl = "https://images.unsplash.com/photo-1566665797739-1674de7a421a?auto=format&fit=crop&w=1000&q=80" },
                new Room { Name = "Standard Queen 102", RoomType = "Standard", Capacity = 2, PricePerNight = 2200, Description = "Simple, bright room with queen bed and all essential amenities.", ImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d57733427?auto=format&fit=crop&w=1000&q=80" }
            );
            await db.SaveChangesAsync();
        }

        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "Admin", "Guest" })
            if (!await roles.RoleExistsAsync(role))
                await roles.CreateAsync(new IdentityRole(role));

        var users = services.GetRequiredService<UserManager<IdentityUser>>();
        var email = services.GetRequiredService<IConfiguration>()["SeedAdmin:Email"] ?? "admin@hotel.local";
        var password = services.GetRequiredService<IConfiguration>()["SeedAdmin:Password"] ?? "ChangeMe123";
        var admin = await users.FindByEmailAsync(email);
        if (admin == null)
        {
            admin = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await users.CreateAsync(admin, password);
            if (result.Succeeded) await users.AddToRoleAsync(admin, "Admin");
        }
    }
}
