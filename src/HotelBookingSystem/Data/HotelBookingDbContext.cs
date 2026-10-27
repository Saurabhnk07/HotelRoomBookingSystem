using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Data;

public class HotelBookingDbContext : IdentityDbContext<IdentityUser>
{
    public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : base(options) { }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Room>().HasIndex(r => r.Name).IsUnique();
        builder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Booking>().Property(b => b.Status).HasConversion<string>().HasMaxLength(20);
    }
}
