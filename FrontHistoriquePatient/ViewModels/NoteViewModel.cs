using System.ComponentModel.DataAnnotations;
namespace FrontHistoriquePatient.ViewModels
{
    public class NoteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} doit être rempli")]
        public int PatientId { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        public DateTime DateHeure { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        public string UneNote { get; set; }
    }
}
