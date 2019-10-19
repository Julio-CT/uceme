using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UCEME.Models.ClasesVista
{
    public class BlogVista
    {
        public int IdBlog { get; set; }

        public string Usuario { get; set; }

        [Required(ErrorMessage = "Campo Titulo requerido")]
        public string Titulo { get; set; }

        public System.DateTime Fecha { get; set; }

        public string Foto { get; set; }

        [Required(ErrorMessage = "Campo Texto requerido")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Texto { get; set; }

        public bool? Profesional { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public int Dia { get; set; }

        [Display(Name = "Dia")]
        public string Strmes
        {
            get
            {
                return Utilidades.DiasHoras.MonthName(Mes);
            }
        }
    }
}