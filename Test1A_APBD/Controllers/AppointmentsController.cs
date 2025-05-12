using Microsoft.AspNetCore.Mvc;
using Test1A_APBD.Exceptions;
using Test1A_APBD.Models.DTO;
using Test1A_APBD.Services;

namespace Test1A_APBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public AppointmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentHistory(int id)
        {
            try
            {
                var res = await _dbService.GetAppointmentHistory(id);
                return Ok(res);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}