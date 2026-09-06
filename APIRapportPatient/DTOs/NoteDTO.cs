using System.ComponentModel.DataAnnotations;

namespace APIRapportPatient.DTOs
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime DateHeure { get; set; }
        public string UneNote { get; set; }
    }
}
