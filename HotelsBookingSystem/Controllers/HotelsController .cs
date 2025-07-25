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
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsService hotelsService;

        public HotelsController(IHotelsService service)
        {
            hotelsService = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetHotelByName([FromQuery] string name)
        {
            var hotel = await hotelsService.GetHotelByNameAsync(name);
            if (hotel == null) return NotFound();

            return Ok(hotel);
        }

        [HttpGet("/{id}")]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetHotelById([FromRoute] int id)
        {
            var hotel = await hotelsService.GetHotelByIdAsync(id);
            if (hotel == null) return NotFound();

            return Ok(hotel);
        }
    }
}
