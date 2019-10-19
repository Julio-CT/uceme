using System.ComponentModel.DataAnnotations;

namespace UCEME.Models
{
    public partial class Usuario
    {
        [Display(Name = "Recuerdame")]
        public bool Recordar { get; set; }
    }
}