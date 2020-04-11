using System.ComponentModel.DataAnnotations;

namespace Uceme.Model.Models.ClasesVista
{
    public class CitaVista
    {
        [Display(Name = "Hora")]
        public string Strhora
        {
            get
            {
                return UCEME.Utilidades.DiasHoras.TimeToString(this.Hora);
            }
        }

        [Display(Name = "Dia")]
        public string Strdia
        {
            get
            {
                return UCEME.Utilidades.DiasHoras.EuropeanDay(this.Dia);
            }
        }

        public int IdCita { get; set; }

        [Display(Name = "Dia de la seman")]
        public int Dia { get; set; }

        [Display(Name = "Hora")]
        public decimal Hora { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        public int IdTurno { get; set; }

        [Display(Name = "Hospital")]
        public string Hospital { get; set; }
    }
}