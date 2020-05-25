namespace UCEME.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Uceme.Model.Models;
    using Uceme.Model.Models.ClasesVista;

    public class TecnicasController : SuperController
    {
        //
        // GET: /Tecnicas/

        private static List<TecnicaVista> _conjuntodata;
        private static readonly int _elementospp = 5;

        public TecnicasController()
        {
            _conjuntodata = (from o in this.DbContext.Tecnica
                             orderby o.fecha descending
                             select new TecnicaVista
                             {
                                 IdTecnica = o.idTecnica,
                                 Titulo = o.titulo,
                                 Foto = o.foto,
                                 Texto = o.texto
                             }).ToList();
        }

        private List<TecnicaVista> GetSubconjunto(int pagina = 1)
        {
            var skipRecords = pagina * _elementospp;

            return _conjuntodata.
                Skip(skipRecords).
                Take(_elementospp).ToList();
        }

        public ActionResult Index(int? id)
        {
            //scrolling
            System.Threading.Thread.Sleep(2000);

            var pagina = id ?? 0;

            var data = this.GetSubconjunto(pagina);

            if (this.Request.IsAjaxRequest())
                return this.PartialView("Subconjunto", data);

            return this.View(data);
        }

        //nuevo!! para traer un solo articulo (para cuando linkemos desde fb o tw)
        public ActionResult Uno(int id)
        {
            var data = (from o in this.DbContext.Tecnica
                        where o.idTecnica == id
                        select new TecnicaVista
                        {
                            IdTecnica = o.idTecnica,
                            Titulo = o.titulo,
                            Foto = o.foto,
                            Texto = o.texto
                        }).ToList();

            return this.View("Index", data);
        }

        [Authorize]
        public ActionResult Anadir()
        {
            var blog = new TecnicaVista();
            return this.View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Anadir(TecnicaVista model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
            {
                if (fichero != null && fichero.ContentLength > 0)
                {
                    var blog = new Tecnica
                    {
                        titulo = model.Titulo,
                        fecha = DateTime.Now,
                        texto = model.Texto,
                        foto = ""
                    };

                    this.DbContext.Tecnica.Add(blog);
                    this.DbContext.SaveChanges();

                    try
                    {
                        var nombre = "Tecnica" + blog.idTecnica;
                        var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", StringComparison.CurrentCulture));
                        var ruta = this.Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                        fichero.SaveAs(ruta);
                        blog.foto = "~/uploads/fotos/" + nombre + extension;

                        this.DbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                        this.DbContext.Tecnica.Remove(blog);
                        this.DbContext.SaveChanges();

                        this.ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCode.ErrorAddingItem) + " " + e.Message);
                        return this.RedirectToAction("index", "Tecnicas");
                    }
                }
            }
            return this.RedirectToAction("Index", "Tecnicas");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var blog = (from o in this.DbContext.Tecnica
                        where o.idTecnica == id
                        select new TecnicaVista
                        {
                            IdTecnica = o.idTecnica,
                            Titulo = o.titulo,
                            Foto = o.foto,
                            Texto = o.texto
                        }).FirstOrDefault();

            return this.View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(TecnicaVista model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
            {
                /* Se supone que un usuario solo podrá editar sus blog asi que dejo el usuario sin modificar
                 CustomIdentity cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                 Usuario usu = db.Usuario.FirstOrDefault(oo => oo.login == cus.Email);
                 */

                //Buscamos el blog a modificar...
                var blog = this.DbContext.Tecnica.Find(model.IdTecnica);

                blog.titulo = model.Titulo;
                blog.fecha = DateTime.Now;
                blog.texto = model.Texto;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos la nueva imagen con la misma ruta que tenía antes, solo cambia el nombre
                    var nombre = "Tecnica" + model.IdTecnica;
                    var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", StringComparison.Ordinal));
                    var ruta = this.Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                    fichero.SaveAs(ruta);
                    blog.foto = "~/uploads/fotos/" + nombre + extension;
                }

                this.DbContext.SaveChanges();
            }

            return this.RedirectToAction("Index", "Tecnicas");
        }

        // action para eliminar una entrada en el blog
        [Authorize]
        public ActionResult Eliminar(int id)
        {
            //buscamos el bicho y lo eliminamos
            if (this.ModelState.IsValid)
            {
                var blog = this.DbContext.Tecnica.Find(id);

                //borramos la foto
                var foto = blog.foto;
                var rutacompleta = this.Server.MapPath("~/") + foto.Substring(2);
                System.IO.File.Delete(rutacompleta);

                this.DbContext.Tecnica.Remove(blog);
                this.DbContext.SaveChanges();
            }

            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}