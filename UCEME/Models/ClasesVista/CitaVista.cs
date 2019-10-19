using System.ComponentModel.DataAnnotations;

namespace UCEME.Models.ClasesVista
{
    public class CitaVista
    {
        [Display(Name = "Hora")]
        public string Strhora
        {
            get
            {
                return Utilidades.DiasHoras.TimeToString(Hora);
            }
        }

        [Display(Name = "Dia")]
        public string Strdia
        {
            get
            {
                return Utilidades.DiasHoras.EuropeanDay(Dia);
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