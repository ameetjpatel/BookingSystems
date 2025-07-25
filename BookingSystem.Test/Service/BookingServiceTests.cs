using System;
using HotelsBookingSystem.Context;
using HotelsBookingSystem.Models;
using HotelsBookingSystem.Models.Enums;
using HotelsBookingSystem.Service;
using Microsoft.EntityFrameworkCore;

namespace HotelsBookingSystemTests.Service
{
    public class BookingServiceTests
    {
        private HotelDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new HotelDbContext(options);

            var hotel = new Hotel { Name = "Test Hotel" };
            var room = new Room { Type = RoomType.Double, Capacity = 2, HotelId = hotel.Id };
            context.Hotels.Add(hotel);
            context.Rooms.Add(room);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task Should_CreateBooking_When_Available()
        {
            var context = GetDbContext();
            var service = new BookingService(context);
            var room = context.Rooms.First();

            var booking = await service.CreateBooking(room.Id, 2, DateTime.Today, DateTime.Today.AddDays(2));

            Assert.NotNull(booking);
            Assert.Equal(room.Id, booking.RoomId);
        }

        [Fact]
        public async Task Should_FailBooking_When_Overlapping()
        {
            var context = GetDbContext();
            var room = context.Rooms.First();

            context.Bookings.Add(new Booking
            {
                RoomId = room.Id,
                //Room = room,
                NumberOfGuests = 2,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                BookingReference = Guid.NewGuid().ToString()
            });
            context.SaveChanges();

            var service = new BookingService(context);
            var result = await service.CreateBooking(room.Id, 2, DateTime.Today.AddDays(1), DateTime.Today.AddDays(4));

            Assert.Null(result);
        }

        [Fact]
        public async Task Should_FailBooking_When_Exceeding_Capacity()
        {
            var context = GetDbContext();
            var service = new BookingService(context);
            var room = context.Rooms.First();

            var result = await service.CreateBooking(room.Id, 4, DateTime.Today, DateTime.Today.AddDays(1));
            Assert.Null(result);
        }
    }
}
