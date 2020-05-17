namespace Uceme.Model.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class Usuario
    {
        [Display(Name = "Recuerdame")]
        public bool Recordar { get; set; }
    }
}