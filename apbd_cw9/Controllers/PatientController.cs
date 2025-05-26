using apbd_cw9.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_cw9.Controllers;


    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {
        private readonly IDbService _service;

        public PatientController(IDbService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientDetails(int id)
        {
            try
            {
                var result = await _service.GetPatientDetailsAsync(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
    }
