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
    
    public partial class ItemCurriculum
    {
        public int idItem { get; set; }
        public string Titulo { get; set; }
        public string Fechas { get; set; }
        public string Texto { get; set; }
        public int idCurriculum { get; set; }
    
        public virtual Curriculum Curriculum { get; set; }
    }
}
