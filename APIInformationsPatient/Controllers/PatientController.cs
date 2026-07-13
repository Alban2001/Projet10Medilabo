using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using APIInformationsPatient.Repositories;
using System.Collections;

namespace APIInformationsPatient.Controllers
{
    [Route("api/patient")]
    public class PatientController : Controller
    {
        private IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        [Route("/Patients")]
        public async Task<IActionResult> Patients()
        {
            var patients = await _patientRepository.FindAll();

            return Ok(patients);
        }

        [HttpGet]
        [Route("/Patient/{id}")]
        public async Task<IActionResult> Patient(int id)
        {
            Patient patient = await _patientRepository.FindById(id);

            if (patient == null)
                return NotFound("Patient introuvable");

            return Ok(patient);
        }

        [HttpPost]
        [Route("/Patient")]
        public async Task<IActionResult> Create([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            _patientRepository.Add(patient);

            return Created(string.Empty, patient);
        }

        [HttpPut]
        [Route("/Patient/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Patient patient)
        {
            Patient verifPatient = await _patientRepository.FindById(id);

            if (verifPatient == null)
                return NotFound("Patient introuvable");

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            _patientRepository.Update(patient);

            return Created(string.Empty, patient);
        }

        [HttpDelete]
        [Route("/Patient/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Patient patient = await _patientRepository.FindById(id);

            if (patient == null)
                return NotFound("Patient introuvable");

            _patientRepository.Delete(patient);

            return NoContent();
        }
    }
}
