namespace Uceme.Model.Models.ClasesVista
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class TecnicaVista
    {
        public int IdTecnica { get; set; }

        public string Titulo { get; set; }

        public string Foto { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Texto { get; set; }
    }
}