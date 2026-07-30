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

        public PatientController(PatientAPIService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Route("patients")]
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetPatients();

            return View("Patients", patients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientService.GetPatient(id);
            return View(patient);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel patient)
        {
            try
            {
                await _patientService.CreatePatient(patient);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(patient);
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientViewModel patient)
        {
            try
            {
                await _patientService.UpdatePatient(id, patient);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(patient);
            }
        }

        // GET: PatientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, PatientViewModel patient)
        {
            try
            {
                await _patientService.DeletePatient(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(patient);
            }
        }
    }
}
