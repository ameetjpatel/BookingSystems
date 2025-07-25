using HotelsBookingSystem.Controllers;
using HotelsBookingSystem.Models.Enums;
using HotelsBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelsBookingSystem.Context;
using System;
using HotelsBookingSystem.Service;
using Moq;

namespace HotelsBookingSystemTests.Controllers
{

    public class RoomsControllerTests
    {
        private Mock<IRoomsServices> mockRoomsService;  

        private RoomsController GetControllerContext()
        {
            mockRoomsService = new Mock<IRoomsServices>();
            return new RoomsController(mockRoomsService.Object);
        }

        [Fact]
        public async Task GetAvailableRooms_ReturnsAvailableRoom()
        {
            var controller = GetControllerContext();
            var hotel = new Hotel { Id = 1, Name = "Test Hotel" };
            var expectedResult = new List<Room> {
                        new Room { Id = 1, Type = RoomType.Double, Capacity = 2, HotelId = hotel.Id } };
            mockRoomsService
                .Setup(s => s.GetAvailableRoomsAsync(hotel.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .ReturnsAsync(expectedResult);
            var result = await controller.GetAvailableRooms(hotel.Id, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var rooms = Assert.IsAssignableFrom<System.Collections.IEnumerable>(okResult.Value);
            mockRoomsService.Verify(mockRoomsService => mockRoomsService.GetAvailableRoomsAsync(hotel.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once);
        }
    }
}
