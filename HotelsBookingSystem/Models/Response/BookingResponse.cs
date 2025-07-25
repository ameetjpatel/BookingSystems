using HotelsBookingSystem.Models.Enums;

namespace HotelsBookingSystem.Models.Response
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public string BookingReference { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public HotelResponse Hotel { get; set; }
    }

    public class HotelResponse  
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<RoomResponse> Rooms { get; set; }
    }

    public class RoomResponse   
    {
        public int Id { get; set; }
        public RoomType Type { get; set; }
        public int Capacity { get; set; }
    }
}
