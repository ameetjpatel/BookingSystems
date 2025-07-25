using HotelsBookingSystem.Models.Enums;

namespace HotelsBookingSystem.Models
{
    public class Room
    {
        public int Id { get; set; }
        public required RoomType Type { get; set; }
        public required int Capacity { get; set; }
        public int HotelId { get; set; }
    }
}
