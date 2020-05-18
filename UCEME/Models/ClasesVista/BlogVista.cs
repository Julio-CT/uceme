namespace Uceme.Model.Models.ClasesVista
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class BlogVista
    {
        public int IdBlog { get; set; }

        public string Usuario { get; set; }

        [Required(ErrorMessage = "Campo Titulo requerido")]
        public string Titulo { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Campo Fecha requerido")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

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
                return UCEME.Utilidades.DiasHoras.MonthName(this.Mes);
            }
        }
    }
}