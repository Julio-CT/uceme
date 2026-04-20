using System.ComponentModel.DataAnnotations;

namespace Uceme.Model.Models.Security;

public class RegisterExternalLoginModel
{
    [Required]
    [Display(Name = "Nombre de usuario")]
    public string? UserName { get; set; }

    public string? ExternalLoginData { get; set; }
}
