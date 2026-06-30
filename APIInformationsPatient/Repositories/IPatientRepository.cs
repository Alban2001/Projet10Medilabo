using Models;

namespace APIInformationsPatient.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> FindAll();
        void Add(Patient patient);
        void Update(Patient patient);
        void Delete(Patient patient);
        Task<Patient> FindById(int id);
    }
}
