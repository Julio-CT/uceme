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
    using System.ComponentModel.DataAnnotations;

    public partial class Curriculum
    {
        public Curriculum()
        {
            ////this.ItemCurriculum = new HashSet<ItemCurriculum>();
            ////this.Usuario = new HashSet<Usuario>();
        }

        [Key]
        public int idCurriculum { get; set; }
        public string Titulo { get; set; }
        public string Text { get; set; }

        ////public virtual ICollection<ItemCurriculum> ItemCurriculum { get; set; }
        ////public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
