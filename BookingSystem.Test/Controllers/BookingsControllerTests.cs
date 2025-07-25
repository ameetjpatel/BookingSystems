using HotelsBookingSystem.Controllers;
using HotelsBookingSystem.Models.Enums;
using HotelsBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelsBookingSystem.Context;
using HotelsBookingSystem.Service;
using HotelsBookingSystem.Models.Response;
using AutoMapper;

namespace HotelsBookingSystemTests.Controllers
{
    public class BookingsControllerTests
    {
        IMapper _mapper;

        public BookingsControllerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Booking, BookingResponse>();
                cfg.CreateMap<Hotel, HotelResponse>()
                .ForMember(x => x.Rooms, opt => opt.Ignore());
                cfg.CreateMap<Room, RoomResponse>();
            });

            _mapper = mapperConfig.CreateMapper();
        }
        private HotelDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase("BookingCtrlTest")
            .Options;

            var context = new HotelDbContext(options);
            var hotel = new Hotel { Name = "Test Hotel" };
            context.Hotels.Add(hotel);
            context.SaveChanges();
            var room = new Room { Type = RoomType.Deluxe, Capacity = 4, HotelId = hotel.Id };
            context.Rooms.Add(room);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task BookRoom_ReturnsOk_WhenBookingIsValid()
        {
            var context = GetDbContext();
            var service = new BookingService(context);
            var hotelService = new HotelsService(context);           

            var controller = new BookingsController(service, hotelService, _mapper);

            var room = await context.Rooms.FirstAsync();

            var result = await controller.BookRoom(room.Id, 2, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var okResult = Assert.IsType<OkObjectResult>(result);
            var booking = Assert.IsType<BookingResponse>(okResult.Value);
            Assert.NotNull(booking.BookingReference);
        }

        [Fact]
        public async Task BookRoom_ReturnsBadRequest_WhenInvalid()
        {
            var context = GetDbContext();
            var service = new BookingService(context);
            var hotelService = new HotelsService(context);

            var controller = new BookingsController(service, hotelService, _mapper);

            var result = await controller.BookRoom(999, 2, DateTime.Today, DateTime.Today.AddDays(2));
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [MemberData(nameof(GetInvalidDates))]
        public async Task BookRoom_ReturnsBadRequest_WhenInValidDates(DateTime startDate, DateTime endDate)
        {
            var context = GetDbContext();
            var service = new BookingService(context);
            var hotelService = new HotelsService(context);

            var controller = new BookingsController(service, hotelService, _mapper);

            var result = await controller.BookRoom(context.Rooms.First().Id, 2, startDate, endDate);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        public static TheoryData<DateTime, DateTime> GetInvalidDates()
        {
            var data = new TheoryData<DateTime, DateTime>();
            data.Add(DateTime.Today, DateTime.Today);
            data.Add(DateTime.Today.AddDays(1), DateTime.Today);
            data.Add(DateTime.Today, DateTime.Today.AddDays(-1));
            data.Add(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));
            return data;
        }
    }
}
