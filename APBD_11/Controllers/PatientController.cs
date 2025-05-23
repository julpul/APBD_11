using APBD_11.DTOs;
using APBD_11.Exeptions;
using APBD_11.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IDbService _dbService;

        public PatientController(IDbService dbService)
        {
            _dbService = dbService;
        }
        
        [HttpPost]
        public async Task<IActionResult> PostPrescription([FromBody] NewRecepraDto newRecepraDto)
        {
            try
            {
                await _dbService.AddPrescription(newRecepraDto);
                return Created("", null);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetPatient([FromRoute] int patientId)
        {
            try
            {
                var result = await _dbService.GetPatient(patientId);
                return Ok(result);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}