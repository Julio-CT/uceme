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
    using System.ComponentModel.DataAnnotations;

    public partial class DatosProfesionales
    {
        public DatosProfesionales()
        {
            ////this.Turno = new HashSet<Turno>();
        }

        [Key]
        public int idDatosPro { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string direccion { get; set; }
        public string texto { get; set; }
        public string foto { get; set; }
        public Nullable<bool> activo { get; set; }
    
        ////public virtual ICollection<Turno> Turno { get; set; }
    }
}
