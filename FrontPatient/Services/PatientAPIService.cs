using FrontPatient.ViewModels;

namespace FrontPatient.Services
{
    public class PatientAPIService
    {
        private readonly HttpClient _httpClient;

        public PatientAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PatientViewModel>> GetPatients()
        {
            return await _httpClient.GetFromJsonAsync<List<PatientViewModel>>("");
        }

        public async Task<PatientViewModel> GetPatient(int id)
        {
            return await _httpClient.GetFromJsonAsync<PatientViewModel>($"{id}");
        }

        public async Task CreatePatient(PatientViewModel patient)
        {
            await _httpClient.PostAsJsonAsync("", patient);
        }

        public async Task UpdatePatient(int id, PatientViewModel patient)
        {
            await _httpClient.PutAsJsonAsync($"{id}", patient);
        }

        public async Task DeletePatient(int id)
        {
            await _httpClient.DeleteAsync($"{id}");
        }
    }
}
