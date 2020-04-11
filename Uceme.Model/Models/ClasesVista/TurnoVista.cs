using System.ComponentModel.DataAnnotations;

namespace Uceme.Model.Models.ClasesVista
{
    public class TurnoVista
    {
        [Display(Name = "Hora de inicio")]
        public string Strinicio
        {
            get
            {
                return UCEME.Utilidades.DiasHoras.TimeToString(this.Inicio);
            }
        }

        [Display(Name = "Hora de finalizacion")]
        public string Strfin
        {
            get
            {
                return UCEME.Utilidades.DiasHoras.TimeToString(this.Fin);
            }
        }

        public int IdTurno { get; set; }

        [Display(Name = "Dia de la semana")]
        [Range(1, 7,
        ErrorMessage = "El valor debe estar entre {1} y {2}.")]
        public int Dia { get; set; }

        [Display(Name = "Hora de inicio")]
        public decimal Inicio { get; set; }

        [Display(Name = "Hora de finalizacion")]
        public decimal Fin { get; set; }

        [Display(Name = "Consultas paralelas")]
        public int Paralelas { get; set; }

        [Display(Name = "Consultas por hora")]
        public int Porhora { get; set; }

        public int IdHospital { get; set; }

        public string Hospital { get; set; }
    }
}