using apbd_cw9.DTOs;
using apbd_cw9.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_cw9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController(IDbService service) : ControllerBase
{

    
    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromQuery] int doctorId, [FromBody] PrescriptionPostDto request)
    {
        try
        {
            var prescriptionId = await service.AddPrescriptionAsync(request, doctorId);
            return Ok(new { Message = "Prescription added", IdPrescription = prescriptionId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Server Error");
        }
    }
}

