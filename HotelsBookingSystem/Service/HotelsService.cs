using HotelsBookingSystem.Context;
using HotelsBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelsBookingSystem.Service
{
    public interface IHotelsService
    {
        Task<Hotel?> GetHotelByNameAsync(string name);
        Task<Hotel?> GetHotelByIdAsync(int id);
    }

    public class HotelsService : IHotelsService
    {
        private readonly HotelDbContext _context;
        public HotelsService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Hotel?> GetHotelByNameAsync(string name)
        {
            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(h => h.Name.ToLower().Contains(name.ToLower()));
            if (hotel == null) return null;
            hotel.Rooms = await _context.Rooms
                .Where(r => r.HotelId == hotel.Id)
                .ToListAsync();

            return hotel;
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(h => h.Id == id) ;
            if (hotel == null) return null;
            hotel.Rooms = await _context.Rooms
                .Where(r => r.HotelId == id)
                .ToListAsync(); 

            return hotel;
        }
    }
}
