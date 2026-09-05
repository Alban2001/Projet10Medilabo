using APIRapportPatient.DTOs;
using System.Net.Http.Headers;

namespace APIRapportPatient.Services
{
    public class RapportAPIService
    {
        private readonly HttpClient _httpClient;

        public RapportAPIService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("RapportAPIService");
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

        // Récupération infos patient
        private async Task<PatientDTO> GetPatient(int id)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/api/patient/{id}");

            return await response.Content.ReadFromJsonAsync<PatientDTO>();
        }

        // Récupération liste notes du patient
        private async Task<List<NoteDTO>> GetNotesByPatient(int idPatient)
        {
            var token = await GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/api/historique/notes/{idPatient}");

            return await response.Content.ReadFromJsonAsync<List<NoteDTO>>();
        }

        // Calcul nombre déclencheurs
        private async Task<int> CountDeclencheurs(int idPatient)
        {
            List<string> listDeclencheurs = ["Hémoglobine A1C", "Microalbumine", "Taille", "Poids", "Fumeur", "Fumeuse", "Anormal", "Cholestérol", "Vertiges", "Rechute", "Réaction", "Anticorps"];
            List<NoteDTO> notes = await GetNotesByPatient(idPatient);
            int nombreDeclencheurs = 0;

            foreach (var note in notes)
            {
                foreach (var declencheur in listDeclencheurs)
                {
                    if (note.UneNote.Contains(declencheur, StringComparison.OrdinalIgnoreCase))
                    {
                        nombreDeclencheurs++;
                    }
                }
            }

            return nombreDeclencheurs;
        }

        public async Task<RapportDTO> GetRapport(int idPatient)
        {
            string NiveauRisque = "Aucun risque";   // par défaut

            // Informations patient et notes
            PatientDTO patient = await GetPatient(idPatient);

            int agePatient = DateTime.Today.Year - patient.DateNaissance.Year;

            if (patient.DateNaissance.Date > DateTime.Today.AddYears(-agePatient))
            {
                agePatient--;
            }

            int nbrDeclencheurs = await CountDeclencheurs(idPatient);
            string genrePatient = patient.Genre;

            // Evaluation niveau de risque

            // Pour les patients de plus de 30ans
            if (agePatient > 30 && (nbrDeclencheurs >= 2 && nbrDeclencheurs <= 5)) NiveauRisque = "Risque limité";
            if (agePatient > 30 && nbrDeclencheurs >= 6) NiveauRisque = "Danger";
            if (agePatient > 30 && nbrDeclencheurs >= 8) NiveauRisque = "Apparition précoce";

            // Pour les patients de moins de 30ans
            if (genrePatient == "Homme" && (agePatient < 30 && nbrDeclencheurs >= 3)) NiveauRisque = "Danger";
            if (genrePatient == "Femme" && (agePatient < 30 && nbrDeclencheurs >= 4)) NiveauRisque = "Danger";
            if (genrePatient == "Homme" && (agePatient < 30 && nbrDeclencheurs >= 5)) NiveauRisque = "Apparition précoce";
            if (genrePatient == "Femme" && (agePatient < 30 && nbrDeclencheurs >= 7)) NiveauRisque = "Apparition précoce";

            RapportDTO rapportPatient = new RapportDTO();
            rapportPatient.PatientId = idPatient;
            rapportPatient.NiveauRisque = NiveauRisque;
            rapportPatient.NombreDeclencheurs = nbrDeclencheurs;

            return rapportPatient;
        }
    }
}
