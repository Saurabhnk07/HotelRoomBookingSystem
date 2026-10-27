/*
  HotelBookingDB schema and sample data.
  This script creates the business tables. ASP.NET Core Identity tables are created by
  the app on first run via EnsureCreated; for a production system use EF migrations.
*/
IF DB_ID(N'HotelBookingDB') IS NULL CREATE DATABASE HotelBookingDB;
GO
USE HotelBookingDB;
GO
IF OBJECT_ID('dbo.Bookings','U') IS NOT NULL DROP TABLE dbo.Bookings;
IF OBJECT_ID('dbo.Rooms','U') IS NOT NULL DROP TABLE dbo.Rooms;
GO
CREATE TABLE dbo.Rooms(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Rooms PRIMARY KEY,
    Name NVARCHAR(80) NOT NULL,
    RoomType NVARCHAR(30) NOT NULL,
    Capacity INT NOT NULL,
    PricePerNight DECIMAL(10,2) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    ImageUrl NVARCHAR(500) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Rooms_IsActive DEFAULT 1,
    CONSTRAINT UQ_Rooms_Name UNIQUE(Name),
    CONSTRAINT CK_Rooms_Capacity CHECK(Capacity BETWEEN 1 AND 10),
    CONSTRAINT CK_Rooms_Price CHECK(PricePerNight > 0)
);
CREATE TABLE dbo.Bookings(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Bookings PRIMARY KEY,
    RoomId INT NOT NULL,
    GuestName NVARCHAR(100) NOT NULL,
    GuestEmail NVARCHAR(150) NOT NULL,
    CheckIn DATE NOT NULL,
    CheckOut DATE NOT NULL,
    Guests INT NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(20) NOT NULL CONSTRAINT DF_Bookings_Status DEFAULT 'Confirmed',
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_Bookings_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Bookings_Rooms FOREIGN KEY(RoomId) REFERENCES dbo.Rooms(Id),
    CONSTRAINT CK_Bookings_Dates CHECK(CheckOut > CheckIn),
    CONSTRAINT CK_Bookings_Guests CHECK(Guests BETWEEN 1 AND 10),
    CONSTRAINT CK_Bookings_Status CHECK(Status IN ('Pending','Confirmed','Cancelled'))
);
GO
INSERT INTO dbo.Rooms(Name,RoomType,Capacity,PricePerNight,Description,ImageUrl,IsActive) VALUES
(N'Deluxe King 101',N'Deluxe',2,3500,N'Comfortable king bed, work desk, Wi-Fi and ensuite bathroom.',N'https://images.unsplash.com/photo-1611892440504-42a792e24d32?auto=format&fit=crop&w=1000&q=80',1),
(N'Family Suite 201',N'Suite',4,6200,N'Spacious suite for families with a sitting area and city views.',N'https://images.unsplash.com/photo-1566665797739-1674de7a421a?auto=format&fit=crop&w=1000&q=80',1),
(N'Standard Queen 102',N'Standard',2,2200,N'Simple, bright room with queen bed and all essential amenities.',N'https://images.unsplash.com/photo-1590490360182-c33d57733427?auto=format&fit=crop&w=1000&q=80',1);
GO
