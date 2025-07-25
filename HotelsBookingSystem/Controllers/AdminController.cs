using Microsoft.AspNetCore.Mvc;
using HotelsBookingSystem.Service;

namespace HotelsBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService service) => adminService = service;

        [HttpPost("reset")]
        public async Task<IActionResult> Reset()
        {
            await adminService.ClearDataAsync();
            return Ok("Data reset.");
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            adminService.SeedDatasetAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    throw new Exception("Error seeding data: " + task.Exception?.Message);
                }
            });

            return Ok("Seeded.");
        }
    }
}
