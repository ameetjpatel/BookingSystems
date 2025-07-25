using HotelsBookingSystem.Models;
using HotelsBookingSystem.Models.Response;

namespace HotelsBookingSystem.Profile
{
    public class BookingProfile : AutoMapper.Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingResponse>()
                .ForMember(x => x.Hotel, opt => opt.Ignore());
            CreateMap<Hotel, HotelResponse>()
                .ForMember(x =>x.Rooms, opt => opt.Ignore());
            CreateMap<Room, RoomResponse>();
        }
    }
}
