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

    public partial class Fotos
    {
        [Key]
        public int idFoto { get; set; }
        public string nombre { get; set; }
        public string texto { get; set; }
        public Nullable<bool> destacada { get; set; }
        public Nullable<int> posicion { get; set; }
    }
}
