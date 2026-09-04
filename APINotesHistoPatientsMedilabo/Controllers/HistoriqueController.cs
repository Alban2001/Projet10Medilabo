using APINotesHistoPatientsMedilabo.Models;
using APINotesHistoPatientsMedilabo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APINotesHistoPatientsMedilabo.Controllers
{
    [Route("api/historique")]
    [ApiController]
    [Authorize]
    public class HistoriqueController : ControllerBase
    {
        private readonly NoteRepository _repository;

        public HistoriqueController(NoteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("notes/{idPatient}")]
        public async Task<IActionResult> notes(int idPatient)
        {
            var notes = await _repository.GetAllByPatientAsync(idPatient);

            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> note(int id)
        {
            var note = await _repository.GetByIdAsync(id);

            if (note == null)
                return NotFound();

            return Ok(note);
        }

        [HttpGet("nextid")]
        public async Task<ActionResult<int>> GetNextId()
        {
            var nextId = await _repository.GetNextIdAsync();

            return Ok(nextId);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Note note)
        {
            await _repository.CreateAsync(note);

            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Note note)
        {
            // Vérifie que l'ID de l'URL correspond à celui de la note
            if (id != note.Id)
            {
                return BadRequest("L'identifiant de la note ne correspond pas.");
            }

            await _repository.UpdateAsync(note);

            return Ok(note);
        }

        // DELETE : api/Note/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rendezVous = await _repository.GetByIdAsync(id);

            if (rendezVous == null)
            {
                return NotFound("Note introuvable.");
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
