using APIRapportPatient.DTOs;
using APIRapportPatient.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRapportPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RapportController : ControllerBase
    {
        private readonly RapportAPIService _rapportService;

        public RapportController(RapportAPIService rapportService)
        {
            _rapportService = rapportService;
        }

        [HttpGet]
        [Route("{idPatient}")]
        public async Task<IActionResult> notes(int idPatient)
        {
            RapportDTO rapport = await _rapportService.GetRapport(idPatient);

            return Ok(rapport);
        }
    }
}
