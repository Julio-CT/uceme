namespace UCEME.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Uceme.Model.Models;

    public class PaginaAmigaController : SuperController
    {
        //
        // GET: /PaginaAmiga/

        public ActionResult Index()
        {
            var data = this.DbContext.PaginaAmiga;
            return this.View(data);
        }

        //nuevo!! para traer un solo articulo (para cuando linkemos desde fb o tw)
        public ActionResult Uno(int id)
        {
            var data = (from o in this.DbContext.PaginaAmiga
                        where o.idPagina == id
                        select new PaginaAmiga
                        {
                            idPagina = o.idPagina,
                            nombre = o.nombre,
                            descripcion = o.descripcion,
                            icono = o.icono,
                            link = o.link
                        }).ToList();

            return this.View("Index", data);
        }

        [Authorize]
        public ActionResult Anadir()
        {
            var newPaginaAmiga = new PaginaAmiga();
            return this.View(newPaginaAmiga);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Anadir(PaginaAmiga model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
            {
                if (fichero != null && fichero.ContentLength > 0)
                {
                    var newPaginaAmiga = new PaginaAmiga
                    {
                        nombre = model.nombre,
                        descripcion = model.descripcion,
                        link = model.link,
                        icono = ""
                    };

                    this.DbContext.PaginaAmiga.Add(newPaginaAmiga);
                    this.DbContext.SaveChanges();

                    try
                    {
                        var nombre = "PagAm" + newPaginaAmiga.idPagina;
                        var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", StringComparison.CurrentCulture));
                        var ruta = this.Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                        fichero.SaveAs(ruta);
                        newPaginaAmiga.icono = "~/uploads/fotos/" + nombre + extension;

                        this.DbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                        this.DbContext.PaginaAmiga.Remove(newPaginaAmiga);
                        this.DbContext.SaveChanges();

                        this.ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                        return this.RedirectToAction("index", "PaginaAmiga");
                    }
                }
            }
            return this.RedirectToAction("Index", "PaginaAmiga");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var selectedPaginaAmiga = this.DbContext.PaginaAmiga.Find(id);

            return this.View(selectedPaginaAmiga);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(PaginaAmiga model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
            {
                /* Se supone que un usuario solo podrá editar sus PaginaAmiga asi que dejo el usuario sin modificar
                 CustomIdentity cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                 Usuario usu = db.Usuario.FirstOrDefault(oo => oo.login == cus.Email);
                 */

                //Buscamos el PaginaAmiga a modificar...
                var selectedPaginaAmiga = this.DbContext.PaginaAmiga.Find(model.idPagina);

                selectedPaginaAmiga.nombre = model.nombre;
                selectedPaginaAmiga.descripcion = model.descripcion;
                selectedPaginaAmiga.link = model.link;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos la nueva imagen con la misma ruta que tenía antes, solo cambia el nombre
                    var nombre = "PagAm" + model.idPagina;
                    var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", StringComparison.CurrentCulture));
                    var ruta = this.Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                    fichero.SaveAs(ruta);
                    selectedPaginaAmiga.icono = "~/uploads/fotos/" + nombre + extension;
                }

                this.DbContext.SaveChanges();
            }
            return this.RedirectToAction("Index", "PaginaAmiga");
        }

        // action para eliminar una entrada en el PaginaAmiga
        [Authorize]
        public ActionResult Eliminar(int id)
        {
            //buscamos el bicho y lo eliminamos
            if (this.ModelState.IsValid)
            {
                var selectedPaginaAmiga = this.DbContext.PaginaAmiga.Find(id);
                this.DbContext.PaginaAmiga.Remove(selectedPaginaAmiga);
                this.DbContext.SaveChanges();
            }
            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}