using FrontPatient.Services;
using FrontPatient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontPatient.Controllers
{
    [Route("historique")]
    public class HistoriqueController : Controller
    {
        private readonly HistoriqueAPIService _noteService;

        public HistoriqueController(HistoriqueAPIService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        [Route("details/{idPatient}")]
        public async Task<IActionResult> Details(int idPatient)
        {
            var listeNotes = await _noteService.GetNotesByPatient(idPatient);

            ViewBag.PatientID = idPatient;

            return View(listeNotes);
        }

        [HttpGet]
        [Route("ajout/{idPatient}")]
        public async Task<IActionResult> Create(int idPatient)
        {
            ViewBag.PatientID = idPatient;

            return View();
        }

        [HttpPost]
        [Route("ajout/{idPatient}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteViewModel note, int idPatient)
        {
            note.DateHeure = DateTime.Now;
            note.Id = await _noteService.GetNextId();
            note.PatientId = idPatient;

            if (!ModelState.IsValid)
            {
                ViewBag.PatientID = idPatient;
                return View(note);
            }

            try
            {
                await _noteService.CreateNote(note);

                return RedirectToAction("Details", new { idPatient = idPatient });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.PatientID = idPatient;
                return View(note);
            }

        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var note = await _noteService.GetNote(id);
            return View(note);
        }

        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteViewModel note)
        {
            var UneNote = await _noteService.GetNote(id);
            note.DateHeure = DateTime.Now;
            note.Id = id;
            note.PatientId = UneNote.PatientId;
            if (!ModelState.IsValid)
            {
                ViewBag.PatientID = note.PatientId;
                return View(note);
            }

            try
            {
                await _noteService.UpdateNote(id, note);

                return RedirectToAction("Details", new { idPatient = note.PatientId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                ViewBag.PatientID = note.PatientId;
                return View(note);
            }
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [Route("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var note = await _noteService.GetNote(id);

            try
            {
                await _noteService.DeleteNote(id);

                return RedirectToAction("Details", new { idPatient = note.PatientId });
            }
            catch
            {
                return View(note);
            }
        }
    }
}
