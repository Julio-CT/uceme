namespace Uceme.Model.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }
}
