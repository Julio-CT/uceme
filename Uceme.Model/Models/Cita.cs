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
    
    public partial class Cita
    {
        public int idCita { get; set; }
        public int dia { get; set; }
        public decimal hora { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public int idTurno { get; set; }
    
        public virtual Turno Turno { get; set; }
    }
}
