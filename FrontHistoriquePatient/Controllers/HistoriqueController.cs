using FrontHistoriquePatient.Services;
using FrontHistoriquePatient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontHistoriquePatient.Controllers
{
    [Route("historique")]
    public class HistoriqueController : Controller
    {
        private readonly HistoriqueAPIService _noteService;
        private readonly PatientAPIService _patientService;

        public HistoriqueController(HistoriqueAPIService noteService, PatientAPIService patientService)
        {
            _noteService = noteService;
            _patientService = patientService;
        }

        [HttpGet]
        [Route("patients")]
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetPatients();

            return View("Patients", patients);
        }

        [HttpGet]
        [Route("details/{idPatient}")]
        public async Task<IActionResult> Details(int idPatient)
        {
            var listeNotes = await _noteService.GetNotesByPatient(idPatient);
            return View(listeNotes);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteViewModel note)
        {
            if (!ModelState.IsValid)
            {
                return View(note);
            }

            try
            {
                await _noteService.CreateNote(note);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(note);
            }

        }

        [HttpGet]
        [Route("note/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var note = await _noteService.GetNote(id);
            return View(note);
        }

        [HttpPost]
        [Route("note/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteViewModel note)
        {
            if (!ModelState.IsValid)
            {
                return View(note);
            }

            try
            {
                await _noteService.UpdateNote(id, note);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(note);
            }
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [Route("note/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _noteService.DeleteNote(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var note = await _noteService.GetNote(id);
                return View(note);
            }
        }
    }
}
