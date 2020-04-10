using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uceme.Model.Models;
using Uceme.Model.Models.ClasesVista;

namespace UCEME.Controllers
{
    public class ServiciosController : SuperController
    {
        //
        // GET: /Servicios/

        public ActionResult Index()
        {
            var data = DbContext.Servicio.Select(o => new ServicioVista
            {
                IdServicio = o.idServicio,
                Nombre = o.nombre,
                Foto = o.foto,
                Text = o.text,
                Cabecera = o.cabecera
            });

            return View(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult CrearServicio(ServicioVista model, HttpPostedFileBase fichero)
        {
            var ser = new Servicio
            {
                nombre = model.Nombre,
                text = model.Text,
                cabecera = model.Cabecera,
            };

            DbContext.Servicio.Add(ser);
            DbContext.SaveChanges();

            if (fichero != null && fichero.ContentLength > 0)
            {
                try
                {
                    //guardamos el fichero de la foto con nombre ser + id
                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.CurrentCulture));
                    var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\ser" + ser.idServicio + extension;
                    fichero.SaveAs(rutacompleta);
                    ser.foto = "~/uploads/fotos/ser" + ser.idServicio + extension;
                }
                catch (Exception e)
                {
                    //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                    DbContext.Servicio.Remove(ser);
                    DbContext.SaveChanges();

                    ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                    return RedirectToAction("index", "Servicios");
                }
            }

            DbContext.SaveChanges();

            return RedirectToAction("index");
        }

        [Authorize]
        public ActionResult EditarServicio(int id)
        {
            var o = DbContext.Servicio.Find(id);

            var data = new ServicioVista
            {
                IdServicio = o.idServicio,
                Nombre = o.nombre,
                Foto = o.foto,
                Text = o.text,
                Cabecera = o.cabecera
            };

            return View(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult EditarServicio(ServicioVista model, HttpPostedFileBase fichero)
        {
            var ser = DbContext.Servicio.Find(model.IdServicio);

            ser.nombre = model.Nombre;
            ser.text = model.Text;
            ser.cabecera = model.Cabecera;

            if (fichero != null && fichero.ContentLength > 0)
            {
                //guardamos el fichero de la foto con nombre ser + id
                var nombreFichero = fichero.FileName;
                var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.CurrentCulture));
                var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\ser" + ser.idServicio + extension;
                fichero.SaveAs(rutacompleta);
                ser.foto = "~/uploads/fotos/ser" + ser.idServicio + extension;
            }

            DbContext.SaveChanges();

            return RedirectToAction("index");
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var data = DbContext.Servicio.Find(id);

            //borramos la foto
            var foto = data.foto;
            var rutacompleta = Server.MapPath("~/") + foto.Substring(2);
            System.IO.File.Delete(rutacompleta);

            DbContext.Servicio.Remove(data);
            DbContext.SaveChanges();
            return RedirectToAction("index");
        }
    }
}