using Microsoft.AspNetCore.Identity;

namespace APIAuthenticationMediLabo.Models
{
    public class User : IdentityUser
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
    }
}
