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

    public partial class Blog
    {
        [Key]
        public int idBlog { get; set; }
        public string titulo { get; set; }
        public System.DateTime fecha { get; set; }
        public string foto { get; set; }
        public string texto { get; set; }
        public Nullable<bool> profesional { get; set; }
        public string slug { get; set; }
        public string seoTitle { get; set; }
        public string metaDescription { get; set; }

        public int idUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
