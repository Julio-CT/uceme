using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Uceme.Model.Models.ClasesVista
{
    public class TecnicaVista
    {
        public int IdTecnica { get; set; }

        public string Titulo { get; set; }

        public string Foto { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Texto { get; set; }
    }
}