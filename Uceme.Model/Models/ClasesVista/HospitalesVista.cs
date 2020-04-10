using System.Collections.Generic;
using System.Web.Mvc;

namespace Uceme.Model.Models.ClasesVista
{
    public class HospitalesVista
    {
        public string IdHospital { get; set; }

        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string Direccion { get; set; }

        [AllowHtml]
        public string Texto { get; set; }

        public string Foto { get; set; }

        public List<CompaniasVista> Companies { get; set; }

        public List<CompaniasSelectorVista> ListaCompanias { get; set; }
    }
}