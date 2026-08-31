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
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net.Http;

namespace APIInformationsPatient.Controllers
{
    [ApiController]
    [Route("api/patient")]
    [Authorize]
    public class PatientController : Controller
    {
        private IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        [Route("patients")]
        public async Task<IActionResult> Patients()
        {
            var patients = await _patientRepository.FindAll();

            return Ok(patients);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Patient(int id)
        {
            Patient patient = await _patientRepository.FindById(id);

            if (patient == null)
                return NotFound("Patient introuvable");

            return Ok(patient);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            _patientRepository.Add(patient);

            return Created(string.Empty, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Patient patient)
        {
            if (id != patient.Id)
                return NotFound("Patient introuvable");

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            _patientRepository.Update(patient);
            
            return Ok(patient);
        }

        [HttpDelete]
        [Route("{id}")]
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
