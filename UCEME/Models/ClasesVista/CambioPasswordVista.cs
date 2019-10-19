using System.ComponentModel.DataAnnotations;

namespace UCEME.Models.ClasesVista
{
    public class CambioPasswordVista
    {
        [Display(Name = "Usuario actual")]
        public string UsuarioActual { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string PasswordNueva { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("PasswordNueva", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string PasswordRepite { get; set; }
    }
}