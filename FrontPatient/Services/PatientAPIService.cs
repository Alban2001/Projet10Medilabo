using FrontPatient.DTOs;
using Azure.Core;
using FrontPatient.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

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
            var login = "";
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "/api/authentication/login",
                    new LoginDTO
                    {
                        Email = "admin@medilabo.fr",
                        Password = "Admin126754?!"
                    });

                login = await response.Content.ReadAsStringAsync();


            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter("log.txt"))
                {
                    sw.WriteLine(ex.Message);
                }

            }
            return login;
        }

        public async Task<List<PatientViewModel>> GetPatients()
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/patient/patients");

            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            // Si l'API ne renvoie rien
            if (string.IsNullOrWhiteSpace(content))
            {
                return new List<PatientViewModel>();
            }

            // Désérialisation du JSON
            return JsonSerializer.Deserialize<List<PatientViewModel>>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<PatientViewModel>();
        }

        public async Task<PatientViewModel> GetPatient(int id)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/api/patient/{id}");

            return await response.Content.ReadFromJsonAsync<PatientViewModel>();
        }

        public async Task<RapportDTO> GetRapportPatient(int idPatient)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/api/rapport/{idPatient}");

            return await response.Content.ReadFromJsonAsync<RapportDTO>();
        }

        public async Task<PatientViewModel?> CreatePatient(PatientViewModel patient)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("/api/patient", patient);

            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<PatientViewModel>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task UpdatePatient(int id, PatientViewModel patient)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync(
                $"/api/patient/{id}",
                patient);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Erreur API Update : {(int)response.StatusCode} {response.StatusCode}\n{content}");
            }
        }

        public async Task DeletePatient(int id)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            await _httpClient.DeleteAsync($"/api/patient/{id}");
        }
    }
}
