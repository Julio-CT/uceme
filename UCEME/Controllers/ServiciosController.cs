namespace UCEME.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Uceme.Model.Models;
    using Uceme.Model.Models.ClasesVista;

    public class ServiciosController : SuperController
    {
        //
        // GET: /Servicios/

        public ActionResult Index()
        {
            var data = this.DbContext.Servicio.Select(o => new ServicioVista
            {
                IdServicio = o.idServicio,
                Nombre = o.nombre,
                Foto = o.foto,
                Text = o.text,
                Cabecera = o.cabecera
            });

            return this.View(data);
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

            this.DbContext.Servicio.Add(ser);
            this.DbContext.SaveChanges();

            if (fichero != null && fichero.ContentLength > 0)
            {
                try
                {
                    //guardamos el fichero de la foto con nombre ser + id
                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.CurrentCulture));
                    var rutacompleta = this.Server.MapPath("~/uploads/fotos") + @"\ser" + ser.idServicio + extension;
                    fichero.SaveAs(rutacompleta);
                    ser.foto = "~/uploads/fotos/ser" + ser.idServicio + extension;
                }
                catch (Exception e)
                {
                    //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                    this.DbContext.Servicio.Remove(ser);
                    this.DbContext.SaveChanges();

                    this.ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                    return this.RedirectToAction("index", "Servicios");
                }
            }

            this.DbContext.SaveChanges();

            return this.RedirectToAction("index");
        }

        [Authorize]
        public ActionResult EditarServicio(int id)
        {
            var o = this.DbContext.Servicio.Find(id);

            var data = new ServicioVista
            {
                IdServicio = o.idServicio,
                Nombre = o.nombre,
                Foto = o.foto,
                Text = o.text,
                Cabecera = o.cabecera
            };

            return this.View(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult EditarServicio(ServicioVista model, HttpPostedFileBase fichero)
        {
            var ser = this.DbContext.Servicio.Find(model.IdServicio);

            ser.nombre = model.Nombre;
            ser.text = model.Text;
            ser.cabecera = model.Cabecera;

            if (fichero != null && fichero.ContentLength > 0)
            {
                //guardamos el fichero de la foto con nombre ser + id
                var nombreFichero = fichero.FileName;
                var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.CurrentCulture));
                var rutacompleta = this.Server.MapPath("~/uploads/fotos") + @"\ser" + ser.idServicio + extension;
                fichero.SaveAs(rutacompleta);
                ser.foto = "~/uploads/fotos/ser" + ser.idServicio + extension;
            }

            this.DbContext.SaveChanges();

            return this.RedirectToAction("index");
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var data = this.DbContext.Servicio.Find(id);

            //borramos la foto
            var foto = data.foto;
            var rutacompleta = this.Server.MapPath("~/") + foto.Substring(2);
            System.IO.File.Delete(rutacompleta);

            this.DbContext.Servicio.Remove(data);
            this.DbContext.SaveChanges();
            return this.RedirectToAction("index");
        }
    }
}