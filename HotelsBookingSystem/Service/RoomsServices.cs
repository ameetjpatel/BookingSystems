using HotelsBookingSystem.Context;
using HotelsBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelsBookingSystem.Service
{
    public interface IRoomsServices
    {
        Task<List<Room>> GetRoomsByHotelAsync(int id);
        Task<Room> GetRoomByIdAsync(int id);

        Task<List<Room>> GetAvailableRoomsAsync(int hotelId, DateTime from, DateTime to, int guests);
    }

    public class RoomsServices : IRoomsServices
    {
        private readonly HotelDbContext _context;
        public RoomsServices(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetRoomsByHotelAsync(int hotelId)
        {
            return await this._context.Rooms.Where(x => x.HotelId == hotelId).ToListAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    return task.Result;
                }
                else
                {
                    throw new Exception("Room not found");
                }
            });

        }

        public async Task<List<Room>> GetAvailableRoomsAsync(int hotelId, DateTime from, DateTime to, int guests)
        {
            var rooms = await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.Capacity >= guests)
                .ToListAsync();

            var availableRoomsIds = await _context.Bookings
                .Where(b => to <= b.StartDate || from >= b.EndDate)
                .Select(s => s.RoomId).ToListAsync();

            return rooms.Where(r => !availableRoomsIds.Contains(r.Id)).ToList();
        }
    }
}
