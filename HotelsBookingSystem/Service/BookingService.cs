using HotelsBookingSystem.Context;
using HotelsBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace HotelsBookingSystem.Service
{
    public interface IBookingService
    {
        Task<Booking?> CreateBooking(int roomId, int guests, DateTime start, DateTime end);

        Task<Booking?> GetBooking(string reference);
    }
    public class BookingService : IBookingService
    {
        private readonly HotelDbContext _context;

        public BookingService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetBooking(string reference)
        {
            Booking? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingReference == reference);
            if (booking == null) return null;

            booking.Room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == booking.RoomId);
            booking.Hotel = await _context.Hotels.FirstOrDefaultAsync(h => booking.Room != null && h.Id == booking.Room.HotelId);
            return booking;
        }

        public async Task<Booking?> CreateBooking(int roomId, int guests, DateTime start, DateTime end)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null || guests > room.Capacity)
                return null;

            bool overlaps = _context.Bookings.Any(b => start < b.EndDate && end > b.StartDate && b.RoomId == roomId);
            if (overlaps) return null;

            var booking = new Booking
            {
                RoomId = roomId,
                //Room = room,
                NumberOfGuests = guests,
                StartDate = start,
                EndDate = end,
                BookingReference = Guid.NewGuid().ToString().Substring(0, 8)
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }
    }
}
