namespace UCEME.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Uceme.Model.Models;
    using Uceme.Model.Models.ClasesVista;

    public class TerminosController : SuperController
    {
        //
        // GET: /Terminos/

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Dicciopinta(string busqueda)
        {
            //sacamos una lista con todos los terminos y un viewbag con las iniciales

            //si tenemos busqueda, hay que filtrar

            var data = busqueda != null ? this.DbContext.Termino.Where(o => o.nombre.Contains(busqueda)).OrderBy(o => o.nombre).ToList() : this.DbContext.Termino.OrderBy(o => o.nombre).ToList();

            //creamos el abecedario

            var abecedario = new List<string>(new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
            this.ViewBag.abecedario = abecedario;

            //sacamos las iniciales, la primera letra del nombre sin repetir
            var iniciales = data.OrderBy(o => o.nombre).Select(o => o.nombre.Substring(0, 1).ToUpper()).Distinct().ToList();

            this.ViewBag.iniciales = iniciales;

            var datacorto = new List<TerminoMinVista>();
            foreach (var ter in data)
            {
                var tercor = new TerminoMinVista
                {
                    IdTermino = ter.idTermino,
                    Nombre = ter.nombre,
                    Textocorto = this.Limpia(ter.texto.Length > 80 ? ter.texto.Substring(0, 80) : ter.texto)
                };

                datacorto.Add(tercor);
            }

            return this.View(datacorto);
        }

        private string Limpia(string strTexto)
        {
            var result = strTexto;
            result = System.Text.RegularExpressions.Regex.Replace(result, "<((.|\n)*?)>", "");
            result = System.Text.RegularExpressions.Regex.Replace(result, "&aacute;", "a");
            result = System.Text.RegularExpressions.Regex.Replace(result, "&eacute;", "e");
            result = System.Text.RegularExpressions.Regex.Replace(result, "&iacute;", "i");
            result = System.Text.RegularExpressions.Regex.Replace(result, "&oacute;", "o");
            result = System.Text.RegularExpressions.Regex.Replace(result, "&uacute;", "u");
            result = System.Text.RegularExpressions.Regex.Replace(result, "&ntilde;", "ñ");

            return result;
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult CrearTermino(Termino model, HttpPostedFileBase fichero)
        {
            var ter = new Termino
            {
                nombre = model.nombre,
                texto = model.texto,
                link = model.link
            };
            this.DbContext.Termino.Add(ter);
            this.DbContext.SaveChanges();

            if (fichero != null && fichero.ContentLength > 0)
            {
                try
                {
                    //guardamos el fichero de la foto con nombre ter + id
                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.Ordinal));
                    var rutacompleta = this.Server.MapPath("~/uploads/fotos") + @"\ter" + ter.idTermino + extension;
                    fichero.SaveAs(rutacompleta);
                    ter.foto = "~/uploads/fotos/ter" + ter.idTermino + extension;
                }
                catch (Exception e)
                {
                    //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                    this.DbContext.Termino.Remove(ter);
                    this.DbContext.SaveChanges();

                    this.ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCode.ErrorAddingItem) + " " + e.Message);
                    return this.RedirectToAction("index", "Terminos");
                }
            }

            this.DbContext.SaveChanges();

            return this.RedirectToAction("index");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var data = this.DbContext.Termino.Find(id);

            return this.View(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(Termino model, HttpPostedFileBase fichero)
        {
            var ter = this.DbContext.Termino.Find(model.idTermino);

            ter.nombre = model.nombre;
            ter.texto = model.texto;
            ter.link = model.link;

            if (fichero != null && fichero.ContentLength > 0)
            {
                //guardamos el fichero de la foto con nombre ter + id
                var nombreFichero = fichero.FileName;
                var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.Ordinal));
                var rutacompleta = this.Server.MapPath("~/uploads/fotos") + @"\ter" + ter.idTermino + extension;
                fichero.SaveAs(rutacompleta);
                ter.foto = "~/uploads/fotos/comp" + ter.idTermino + extension;
            }

            this.DbContext.SaveChanges();

            return this.RedirectToAction("index");
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var data = this.DbContext.Termino.Find(id);
            this.DbContext.Termino.Remove(data);
            this.DbContext.SaveChanges();
            return this.RedirectToAction("index");
        }

        public ActionResult Detalles(int id)
        {
            var data = this.DbContext.Termino.Find(id);
            return this.View(data);
        }
    }
}