using FrontPatient.Services;
using FrontPatient.ViewModels;

namespace FrontPatient.Data
{
    public class DbSeeder
    {
        private readonly PatientAPIService _patientAPIService;
        private readonly HistoriqueAPIService _historiqueAPIService;

        public DbSeeder(
            PatientAPIService patientAPIService,
            HistoriqueAPIService historiqueAPIService)
        {
            _patientAPIService = patientAPIService;
            _historiqueAPIService = historiqueAPIService;
        }

        public async Task SeedAsync()
        {
            var patientsExistants = await _patientAPIService.GetPatients();

            if (patientsExistants.Any())
            {
                return;
            }

            var patients = new List<PatientViewModel>
{
                new PatientViewModel
                {
                    Id = 1,
                    Nom = "Martin",
                    Prenom = "Marie",
                    DateNaissance = new DateTime(1966, 12, 31),
                    Genre = "Femme",
                    NumeroRue = "1",
                    NomRue = "Brookside St",
                    CodePostal = 00001,
                    Ville = "New York",
                    NumeroTelephone = "100-222-3333"
                },
                new PatientViewModel
                {
                    Id = 2,
                    Nom = "Dupont",
                    Prenom = "Jean",
                    DateNaissance = new DateTime(1945, 06, 24),
                    Genre = "Homme",
                    NumeroRue = "2",
                    NomRue = "High St",
                    CodePostal = 00002,
                    Ville = "Philadelphie",
                    NumeroTelephone = "200-333-4444"
                },
                new PatientViewModel
                {
                    Id = 3,
                    Nom = "Sommer",
                    Prenom = "Lucas",
                    DateNaissance = new DateTime(2004, 06, 18),
                    Genre = "Homme",
                    NumeroRue = "3",
                    NomRue = "Club Road",
                    CodePostal = 00003,
                    Ville = "Miami",
                    NumeroTelephone = "300-444-5555"
                },
                new PatientViewModel
                {
                    Id = 4,
                    Nom = "Muller",
                    Prenom = "Emilie",
                    DateNaissance = new DateTime(2002, 06, 28),
                    Genre = "Femme",
                    NumeroRue = "4",
                    NomRue = "Vally Dr",
                    CodePostal = 00003,
                    Ville = "LA",
                    NumeroTelephone = "400-555-6666"
                }
            };


            // =========================
            // INSERTION DES PATIENTS
            // =========================

            var patientsCrees = new List<PatientViewModel>();

            foreach (var patient in patients)
            {
                await _patientAPIService.CreatePatient(patient);
            }


            var notes = new List<NoteViewModel>
{
    new NoteViewModel
    {
        Id = 1,
        PatientId = 1,
        DateHeure = DateTime.Now,
        UneNote = "Le patient déclare qu'il 'se sent très bien' Poids égal ou inférieur au poids recommandé"
    },

    new NoteViewModel
    {
        Id = 2,
        PatientId = 2,
        DateHeure = DateTime.Now,
        UneNote = "Le patient déclare qu'il ressent beaucoup de stress au travail Il se plaint également que son audition est anormale dernièrement"
    },

    new NoteViewModel
    {
        Id = 3,
        PatientId = 2,
        DateHeure = DateTime.Now,
        UneNote = "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois Il remarque également que son audition continue d'être anormale"
    },

    new NoteViewModel
    {
        Id = 4,
        PatientId = 3,
        DateHeure = DateTime.Now,
        UneNote = "Le patient déclare qu'il fume depuis peu"
    },

    new NoteViewModel
    {
        Id = 5,
        PatientId = 3,
        DateHeure = DateTime.Now,
        UneNote = "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière Il se plaint également de crises d’apnée respiratoire anormales Tests de laboratoire indiquant un taux de cholestérol LDL élevé"
    },

    new NoteViewModel
    {
        Id = 6,
        PatientId = 4,
        DateHeure = DateTime.Now,
        UneNote = "Le patient déclare qu'il lui est devenu difficile de monter les escaliers Il se plaint également d’être essoufflé Tests de laboratoire indiquant que les anticorps sont élevés Réaction aux médicaments"
    },

    new NoteViewModel
    {
        Id = 7,
        PatientId = 4,
        DateHeure = DateTime.Now,
        UneNote = "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps"
    },

    new NoteViewModel
    {
        Id = 8,
        PatientId = 4,
        DateHeure = DateTime.Now,
        UneNote = "Le patient déclare avoir commencé à fumer depuis peu Hémoglobine A1C supérieure au niveau recommandé"
    },

    new NoteViewModel
    {
        Id = 9,
        PatientId = 4,
        DateHeure = DateTime.Now,
        UneNote = "Taille, Poids, Cholestérol, Vertige et Réaction"
    }
};


            // =========================
            // INSERTION DES NOTES
            // =========================

            foreach (var note in notes)
            {
                await _historiqueAPIService.CreateNote(note);
            }
        }
    }
}
