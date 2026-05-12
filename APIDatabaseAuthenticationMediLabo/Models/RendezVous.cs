namespace APIDatabaseAuthenticationMediLabo.Models
{
    public class RendezVous
    {
        public int Id { get; set; }
        public DateTime DateHeure { get; set; }
        public string Note { get; set; }
        public Patient UnPatient { get; set; }
        public User UnMedecin { get; set; }
    }
}
