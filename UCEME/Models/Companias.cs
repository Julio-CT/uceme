//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UCEME.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Companias
    {
        public Companias()
        {
            this.DatosProfesionales = new HashSet<DatosProfesionales>();
        }
    
        public int idCompanias { get; set; }
        public string nombre { get; set; }
        public string foto { get; set; }
        public string link { get; set; }
    
        public virtual ICollection<DatosProfesionales> DatosProfesionales { get; set; }
    }
}