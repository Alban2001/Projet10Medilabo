using FrontPatient.DTOs;
using FrontPatient.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FrontPatient.Services
{
    public class HistoriqueAPIService
    {
        private readonly HttpClient _httpClient;

        public HistoriqueAPIService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("HistoriqueAPIService");
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

        public async Task<List<NoteViewModel>> GetNotesByPatient(int idPatient)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/api/historique/notes/{idPatient}");

            return await response.Content.ReadFromJsonAsync<List<NoteViewModel>>();
        }

        public async Task<NoteViewModel> GetNote(int id)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/api/historique/{id}");

            return await response.Content.ReadFromJsonAsync<NoteViewModel>();
        }

        public async Task<int> GetNextId()
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/historique/nextid");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task CreateNote(NoteViewModel note)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("/api/historique", note);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateNote(int id, NoteViewModel note)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync(
                $"/api/historique/{id}",
                note);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Erreur API Update : {(int)response.StatusCode} {response.StatusCode}\n{content}");
            }
        }

        public async Task DeleteNote(int id)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            await _httpClient.DeleteAsync($"/api/historique/{id}");
        }
    }
}
