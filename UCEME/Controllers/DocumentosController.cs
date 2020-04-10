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
            var data = (from o in DbContext.Documento
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

            return View(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Subir(DocumentosVista model, HttpPostedFileBase fichero, string titulo)
        {
            if (model != null && ModelState.IsValid)
            {
                ////sacamos el usuario completo
                var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;

                //sacamos el usuario completo
                var usu = DbContext.Usuario.FirstOrDefault(oo => oo.login == cus.Email);

                if (fichero != null && fichero.ContentLength > 0 && usu != null)
                {
                    var docu = new Documento
                    {
                        idUsuario = usu.idUsuario,
                        nombre = titulo,
                        link = "lo pongo a posteriori"
                    };

                    DbContext.Documento.Add(docu);
                    DbContext.SaveChanges();

                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.Ordinal));
                    var rutacompleta = Server.MapPath("~/uploads/Documentos") + @"\doc" + docu.idDocumento + extension;
                    fichero.SaveAs(rutacompleta);
                    docu.link = "../Uploads/Documentos/doc" + docu.idDocumento + extension;

                    DbContext.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Documentos");
        }

        [Authorize]
        // action para eliminar un documento
        public ActionResult EliminarDocumento(int idItem)
        {
            //buscamos el bicho y lo eliminamos
            if (ModelState.IsValid)
            {
                var docu = DbContext.Documento.Find(idItem);

                //borramos el archivo
                var foto = docu.link;
                var rutacompleta = Server.MapPath("~/") + foto.Substring(2);
                System.IO.File.Delete(rutacompleta);

                //borramos en la bbdd
                DbContext.Documento.Remove(docu);
                DbContext.SaveChanges();
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}