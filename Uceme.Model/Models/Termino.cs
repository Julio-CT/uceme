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
    using System.ComponentModel.DataAnnotations;

    public partial class Termino
    {
        public string nombre { get; set; }
        public string texto { get; set; }
        public string foto { get; set; }
        public string link { get; set; }
        [Key]
        public int idTermino { get; set; }
    }
}
