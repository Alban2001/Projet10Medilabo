namespace APIDatabaseAuthenticationMediLabo.Models
{
    public class Rapport
    {
        public int Id { get; set; }
        public string NiveauRisque { get; set; }
        public double HemoglobineA1C { get; set; }
        public double Microalbumine { get; set; }
        public double Taille { get; set; }
        public double Poids { get; set; }
        public bool Fumeur { get; set; }
        public bool Anormal { get; set; }
        public double Cholesterol { get; set; }
        public bool Vertiges { get; set; }
        public bool Rechute { get; set; }
        public string Reaction { get; set; }
        public string Anticorps { get; set; }
        public Patient UnPatient { get; set; }
    }
}
