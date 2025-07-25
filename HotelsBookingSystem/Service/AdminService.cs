using HotelsBookingSystem.Context;
using HotelsBookingSystem.Models.Enums;
using HotelsBookingSystem.Models;
using NuGet.Packaging;

namespace HotelsBookingSystem.Service
{
    
    public interface IAdminService
    {
        Task<bool> ClearDataAsync();
        Task<bool> SeedDatasetAsync();
    }   

    public class AdminService : IAdminService
    {
        private readonly HotelDbContext _context;

        public AdminService(HotelDbContext context) => _context = context;

        public async Task<bool> ClearDataAsync()
        {
            _context.Bookings.RemoveRange(_context.Bookings);
            _context.Rooms.RemoveRange(_context.Rooms);
            _context.Hotels.RemoveRange(_context.Hotels);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SeedDatasetAsync()
        {
            if (_context.Hotels.Any()) throw new Exception("Already Data avaialble.");

            int counter = 1;
            var hotel = new Hotel { Id = 1, Name = "Sunrise Hotel", Rooms = new List<Room>() };
            hotel.Rooms.AddRange(new[]
            {
            new Room { Id = counter++, Type = RoomType.Single, Capacity = 1 },
            new Room { Id = counter++, Type = RoomType.Single, Capacity = 1 },
            new Room { Id = counter++, Type = RoomType.Double, Capacity = 2 },
            new Room { Id = counter++, Type = RoomType.Double, Capacity = 2 },
            new Room { Id = counter++, Type = RoomType.Deluxe, Capacity = 4 },
            new Room { Id = counter++, Type = RoomType.Deluxe, Capacity = 4 },
            });

            _context.Hotels.Add(hotel);


            var hotel1 = new Hotel { Id = 2, Name = "Day Inn Hotel", Rooms = new List<Room>() };
            hotel1.Rooms.AddRange(new[]
            {
            new Room { Id = counter++, Type = RoomType.Single, Capacity = 1 },
            new Room { Id = counter++, Type = RoomType.Double, Capacity = 2 },
            new Room { Id = counter++, Type = RoomType.Double, Capacity = 2 },
            new Room { Id = counter++, Type = RoomType.Double, Capacity = 2 },
            new Room { Id = counter++, Type = RoomType.Double, Capacity = 2 },
            new Room { Id = counter++, Type = RoomType.Deluxe, Capacity = 4 },
            });

            _context.Hotels.Add(hotel1);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
