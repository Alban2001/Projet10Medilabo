namespace Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateNaissance { get; set; }
        public string Genre { get; set; }
        public string NumeroRue { get; set; }
        public string NomRue { get; set; }
        public int CodePostal { get; set; }
        public string Ville { get; set; }
        public string NumeroTelephone { get; set; }
    }
}
