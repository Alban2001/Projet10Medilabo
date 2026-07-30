using APIAuthenticationMediLabo.DTOs;
using Azure.Core;
using FrontPatient.ViewModels;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FrontPatient.Services
{
    public class PatientAPIService
    {
        private readonly HttpClient _httpClient;

        public PatientAPIService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("PatientAPIService");
        }

        private async Task<string> GetToken()
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/api/authentication/login",
                new LoginDTO
                {
                    Email = "admin@medilabo.fr",
                    Password = "Admin126754?!"
                });

            response.EnsureSuccessStatusCode();

            var login = await response.Content.ReadAsStringAsync();

            return login;
        }

        public async Task<List<PatientViewModel>> GetPatients()
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("JwtBearer", token);

            var response = await _httpClient.GetAsync("/api/patient/patients");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<PatientViewModel>>();
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
