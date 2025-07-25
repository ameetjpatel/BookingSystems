using AutoMapper;
using HotelsBookingSystem.Models;
using HotelsBookingSystem.Models.Response;
using HotelsBookingSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace HotelsBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;
        private readonly IHotelsService _hotelService;

        private IMapper _mapper;

        public BookingsController(IBookingService service, IHotelsService hotelsService, IMapper mapper)
        {
            _service = service;
            _hotelService = hotelsService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BookingResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> BookRoom(int roomId, int guests, DateTime start, DateTime end)
        {
            if (guests <= 0)
                return BadRequest("Number of guests must be greater than zero.");
            if (start >= end)
                return BadRequest("Start date must be before end date.");
            if (start < DateTime.Now)
                return BadRequest("Start date cannot be in the past.");

            var booking = await _service.CreateBooking(roomId, guests, start, end);
            if (booking == null)
                return BadRequest("Cannot book this room.");

            BookingResponse bookingResponse = _mapper.Map<BookingResponse>(booking);
            var hotels = await _hotelService.GetHotelByIdAsync(booking.Room.HotelId);

            if (hotels == null)
                return NotFound("Hotel not found.");
            
            hotels.Rooms = new List<Room>() { booking.Room };
            bookingResponse.Hotel = _mapper.Map<HotelResponse>(hotels);
            return Ok(bookingResponse);
        }

        [HttpGet("{reference}")]
        [ProducesResponseType(typeof(BookingResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBooking(string reference)
        {
            var booking = await _service.GetBooking(reference);
            if (booking == null) return NotFound();

            BookingResponse bookingResponse = _mapper.Map<BookingResponse>(booking);
            Hotel hotel = await _hotelService.GetHotelByIdAsync(booking.Room.HotelId);
            hotel.Rooms = new List<Room>() { booking.Room };
            bookingResponse.Hotel = _mapper.Map<HotelResponse>(hotel);
            return Ok(bookingResponse);
        }
    }
}
