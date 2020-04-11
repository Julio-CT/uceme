using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uceme.Model.Models;
using Uceme.Model.Models.ClasesVista;
using UCEME.Seguridad;

namespace UCEME.Controllers
{
    public class DocumentosController : SuperController
    {
        //
        // GET: /Documentos/

        public ActionResult Index()
        {
            var data = (from o in this.DbContext.Documento
                        select new DocumentosVista
                        {
                            IdDocumento = o.idDocumento,
                            Nombre = o.nombre,
                            Link = o.link,
                            Usuario = o.Usuario.nombre + " " + o.Usuario.apellidos
                        }).ToList();

            for (var i = 0; i < data.Count; i++)
            {
                data.ElementAt(i).Posicion = i;
            }

            return this.View(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Subir(DocumentosVista model, HttpPostedFileBase fichero, string titulo)
        {
            if (model != null && this.ModelState.IsValid)
            {
                ////sacamos el usuario completo
                var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;

                //sacamos el usuario completo
                var usu = this.DbContext.Usuario.FirstOrDefault(oo => oo.login == cus.Email);

                if (fichero != null && fichero.ContentLength > 0 && usu != null)
                {
                    var docu = new Documento
                    {
                        idUsuario = usu.idUsuario,
                        nombre = titulo,
                        link = "lo pongo a posteriori"
                    };

                    this.DbContext.Documento.Add(docu);
                    this.DbContext.SaveChanges();

                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.Ordinal));
                    var rutacompleta = this.Server.MapPath("~/uploads/Documentos") + @"\doc" + docu.idDocumento + extension;
                    fichero.SaveAs(rutacompleta);
                    docu.link = "../Uploads/Documentos/doc" + docu.idDocumento + extension;

                    this.DbContext.SaveChanges();
                }
            }
            return this.RedirectToAction("Index", "Documentos");
        }

        [Authorize]
        // action para eliminar un documento
        public ActionResult EliminarDocumento(int idItem)
        {
            //buscamos el bicho y lo eliminamos
            if (this.ModelState.IsValid)
            {
                var docu = this.DbContext.Documento.Find(idItem);

                //borramos el archivo
                var foto = docu.link;
                var rutacompleta = this.Server.MapPath("~/") + foto.Substring(2);
                System.IO.File.Delete(rutacompleta);

                //borramos en la bbdd
                this.DbContext.Documento.Remove(docu);
                this.DbContext.SaveChanges();
            }
            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}