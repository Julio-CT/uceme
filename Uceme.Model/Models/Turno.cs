//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Uceme.Model.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Turno
    {
        public Turno()
        {
            this.Cita = new HashSet<Cita>();
        }
    
        public int idTurno { get; set; }
        public int dia { get; set; }
        public decimal inicio { get; set; }
        public decimal fin { get; set; }
        public int paralelas { get; set; }
        public int porhora { get; set; }
        public int idHospital { get; set; }
    
        public virtual ICollection<Cita> Cita { get; set; }
        public virtual DatosProfesionales DatosProfesionales { get; set; }
    }
}
