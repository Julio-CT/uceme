namespace Uceme.Model.Models.ClasesVista
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class ServicioVista
    {
        public int IdServicio { get; set; }

        public string Nombre { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Text { get; set; }

        public string Foto { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Cabecera { get; set; }
    }
}