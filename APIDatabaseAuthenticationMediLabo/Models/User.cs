using Microsoft.AspNetCore.Identity;

namespace APIDatabaseAuthenticationMediLabo.Models
{
    public class User : IdentityUser
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public virtual ICollection<RendezVous>? ListeRendezVous { get; set; }
    }
}
