using System.ComponentModel.DataAnnotations;

namespace Uceme.Model.Models
{
    public partial class Usuario
    {
        [Display(Name = "Recuerdame")]
        public bool Recordar { get; set; }
    }
}