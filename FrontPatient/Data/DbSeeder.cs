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

            // =========================
            // PATIENTS
            // =========================

            var patients = new List<PatientViewModel>
            {
                new PatientViewModel
                {
                    Nom = "Martin",
                    Prenom = "Marie",
                    DateNaissance = new DateTime(1966, 12, 31),
                    Genre = "Femme",
                    NumeroRue = "1",
                    NomRue = "Brookside St",
                    CodePostal = 11111,
                    Ville = "New York",
                    NumeroTelephone = "100-222-3333"
                },

                new PatientViewModel
                {
                    Nom = "Dupont",
                    Prenom = "Jean",
                    DateNaissance = new DateTime(1945, 6, 24),
                    Genre = "Homme",
                    NumeroRue = "2",
                    NomRue = "High St",
                    CodePostal = 22222,
                    Ville = "Philadelphie",
                    NumeroTelephone = "200-333-4444"
                },

                new PatientViewModel
                {
                    Nom = "Sommer",
                    Prenom = "Lucas",
                    DateNaissance = new DateTime(2004, 6, 18),
                    Genre = "Homme",
                    NumeroRue = "3",
                    NomRue = "Club Road",
                    CodePostal = 33333,
                    Ville = "Miami",
                    NumeroTelephone = "300-444-5555"
                },

                new PatientViewModel
                {
                    Nom = "Muller",
                    Prenom = "Emilie",
                    DateNaissance = new DateTime(2002, 6, 28),
                    Genre = "Femme",
                    NumeroRue = "4",
                    NomRue = "Vally Dr",
                    CodePostal = 44444,
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
                var patientCree = await _patientAPIService.CreatePatient(patient);

                if (patientCree != null)
                {
                    patientsCrees.Add(patientCree);
                }
            }

            // =========================
            // NOTES
            // =========================
            
            int incrId = await _historiqueAPIService.GetNextId();
            var notes = new List<NoteViewModel>
            {
                new NoteViewModel
                {
                    Id = incrId,
                    PatientId = patientsCrees[0].Id,
                    DateHeure = DateTime.Now,
                    UneNote = "Le patient déclare qu'il 'se sent très bien' Poids égal ou inférieur au poids recommandé"
                },

                new NoteViewModel
                {
                    Id = incrId+1,
                    PatientId = patientsCrees[1].Id,
                    DateHeure = DateTime.Now,
                    UneNote = "Le patient déclare qu'il ressent beaucoup de stress au travail Il se plaint également que son audition est anormale dernièrement"
                },

                new NoteViewModel
                {
                    Id = incrId+2,
                    PatientId = patientsCrees[1].Id,
                    DateHeure = DateTime.Now,
                    UneNote = "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois Il remarque également que son audition continue d'être anormale"
                },

                new NoteViewModel
                {
                    Id = incrId+3,
                    PatientId = patientsCrees[2].Id,
                    DateHeure = DateTime.Now,
                    UneNote = "Le patient déclare qu'il fume depuis peu"
                },

                new NoteViewModel
                {
                    Id = incrId+4,
                    PatientId = patientsCrees[2].Id,
                    DateHeure = DateTime.Now,
                    UneNote = "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière Il se plaint également de crises d’apnée respiratoire anormales Tests de laboratoire indiquant un taux de cholestérol LDL élevé"
                },

                new NoteViewModel
                {
                    Id = incrId+5,
                    PatientId = patientsCrees[3].Id,
                    DateHeure = DateTime.Now,
                    UneNote = "Le patient déclare qu'il lui est devenu difficile de monter les escaliers Il se plaint également d’être essoufflé Tests de laboratoire indiquant que les anticorps sont élevés Réaction aux médicaments"
                },

                new NoteViewModel
                {
                    Id = incrId+6,
                    PatientId = patientsCrees[3].Id,
                    DateHeure = DateTime.Now,
                    UneNote = "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps"
                },

                new NoteViewModel
                {
                    Id = incrId+7,
                    PatientId = patientsCrees[3].Id,
                    DateHeure = DateTime.Now,
                    UneNote = "Le patient déclare avoir commencé à fumer depuis peu Hémoglobine A1C supérieure au niveau recommandé"
                },

                new NoteViewModel
                {
                    Id = incrId+8,
                    PatientId = patientsCrees[3].Id,
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