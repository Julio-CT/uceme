using System.ComponentModel.DataAnnotations;

namespace UCEME.Models.ClasesVista
{
    public class Contacto
    {
        [Required(ErrorMessage = "Campo Nombre requerido")]
        [Display(Name = "Nombre:")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo Email requerido")]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Mensaje requerido")]
        [Display(Name = "Mensaje:")]
        public string Texto { get; set; }
    }
}