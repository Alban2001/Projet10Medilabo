using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace APIInformationsPatient.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        public AppDBContext DbContext { set; get; }

        public PatientRepository(AppDBContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Patient>> FindAll()
        {
            return await DbContext.Patients.ToListAsync();
        }

        public void Add(Patient patient)
        {
            DbContext.Patients.Add(patient);
            DbContext.SaveChanges();
        }

        public void Update(Patient patient)
        {
            DbContext.Patients.Update(patient);
            DbContext.SaveChanges();
        }

        public void Delete(Patient patient)
        {
            DbContext.Patients.Remove(patient);
            DbContext.SaveChanges();
        }

        public async Task<Patient> FindById(int id)
        {
            Patient patient = await DbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
            {
                return null;
            }
            return patient;
        }

    }
}
