using FrontPatient.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FrontPatient.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} doit être rempli")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        [Display(Name = "Date de Naissance")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateNaissance { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        [Display(Name = "Genre", Prompt = "Homme ou Femme")]
        public string Genre { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        [Display(Name = "Numéro Rue")]
        public string NumeroRue { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        [Display(Name = "Nom rue")]
        public string NomRue { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        [Display(Name = "Code Postal")]
        public int CodePostal { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        public string Ville { get; set; }
        [Required(ErrorMessage = "{0} doit être rempli")]
        [Display(Name = "Numéro Téléphone")]
        public string NumeroTelephone { get; set; }

        [ValidateNever]
        public RapportDTO RapportDTO { get; set; } = null;
    }
}
