using FrontPatient.Data;
using FrontPatient.DTOs;
using FrontPatient.Services;
using FrontPatient.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontPatient.Controllers
{
    [Route("patient")]
    public class PatientController : Controller
    {
        private readonly PatientAPIService _patientService;
        private readonly DbSeeder _dbSeeder;

        public PatientController(PatientAPIService patientService, DbSeeder dbSeeder)
        {
            _patientService = patientService;
            _dbSeeder = dbSeeder;
        }

        [HttpGet]
        [Route("patients")]
        public async Task<IActionResult> Index()
        {
            await _dbSeeder.SeedAsync();

            var patients = await _patientService.GetPatients();

            return View("Patients", patients);
        }

        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            RapportDTO unRapport = await _patientService.GetRapportPatient(id);

            var patient = await _patientService.GetPatient(id);
            patient.RapportDTO = unRapport;

            return View(patient);
        }

        [HttpGet]
        [Route("ajout")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("ajout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                await _patientService.CreatePatient(patient);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }

        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientService.GetPatient(id);
            return View(patient);
        }

        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientViewModel patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                await _patientService.UpdatePatient(id, patient);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(patient);
            }
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [Route("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _patientService.DeletePatient(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var patient = await _patientService.GetPatient(id);
                return View(patient);
            }
        }
    }
}
