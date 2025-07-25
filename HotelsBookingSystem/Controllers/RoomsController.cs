using HotelsBookingSystem.Context;
using HotelsBookingSystem.Models;
using HotelsBookingSystem.Models.Response;
using HotelsBookingSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelsBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsServices roomsServices;

        public RoomsController(IRoomsServices services)
        {
            roomsServices = services;
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(List<Room>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAvailableRooms(int hotelId, DateTime from, DateTime to, int guests)
        {
            if (guests <= 0)
                return BadRequest("Number of guests must be greater than zero.");
            if (from >= to)
                return BadRequest("From date must be before to date.");
            if (from < DateTime.Now)
                return BadRequest("From date cannot be in the past.");

            return Ok(await roomsServices.GetAvailableRoomsAsync(hotelId, from, to, guests));
        }
    }
}
