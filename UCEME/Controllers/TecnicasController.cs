using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uceme.Model.Models;
using Uceme.Model.Models.ClasesVista;

namespace UCEME.Controllers
{
    public class TecnicasController : SuperController
    {
        //
        // GET: /Tecnicas/

        private static List<TecnicaVista> _conjuntodata;
        private static readonly int _elementospp = 5;

        public TecnicasController()
        {
            _conjuntodata = (from o in DbContext.Tecnica
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

            var data = GetSubconjunto(pagina);

            if (Request.IsAjaxRequest())
                return PartialView("Subconjunto", data);

            return View(data);
        }

        //nuevo!! para traer un solo articulo (para cuando linkemos desde fb o tw)
        public ActionResult Uno(int id)
        {
            var data = (from o in DbContext.Tecnica
                        where o.idTecnica == id
                        select new TecnicaVista
                        {
                            IdTecnica = o.idTecnica,
                            Titulo = o.titulo,
                            Foto = o.foto,
                            Texto = o.texto
                        }).ToList();

            return View("Index", data);
        }

        [Authorize]
        public ActionResult Anadir()
        {
            var blog = new TecnicaVista();
            return View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Anadir(TecnicaVista model, HttpPostedFileBase fichero)
        {
            if (model != null && ModelState.IsValid)
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

                    DbContext.Tecnica.Add(blog);
                    DbContext.SaveChanges();

                    try
                    {
                        var nombre = "Tecnica" + blog.idTecnica;
                        var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", StringComparison.CurrentCulture));
                        var ruta = Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                        fichero.SaveAs(ruta);
                        blog.foto = "~/uploads/fotos/" + nombre + extension;

                        DbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                        DbContext.Tecnica.Remove(blog);
                        DbContext.SaveChanges();

                        ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                        return RedirectToAction("index", "Tecnicas");
                    }
                }
            }
            return RedirectToAction("Index", "Tecnicas");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var blog = (from o in DbContext.Tecnica
                        where o.idTecnica == id
                        select new TecnicaVista
                        {
                            IdTecnica = o.idTecnica,
                            Titulo = o.titulo,
                            Foto = o.foto,
                            Texto = o.texto
                        }).FirstOrDefault();

            return View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(TecnicaVista model, HttpPostedFileBase fichero)
        {
            if (model != null && ModelState.IsValid)
            {
                /* Se supone que un usuario solo podrá editar sus blog asi que dejo el usuario sin modificar
                 CustomIdentity cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                 Usuario usu = db.Usuario.FirstOrDefault(oo => oo.login == cus.Email);
                 */

                //Buscamos el blog a modificar...
                var blog = DbContext.Tecnica.Find(model.IdTecnica);

                blog.titulo = model.Titulo;
                blog.fecha = DateTime.Now;
                blog.texto = model.Texto;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos la nueva imagen con la misma ruta que tenía antes, solo cambia el nombre
                    var nombre = "Tecnica" + model.IdTecnica;
                    var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", StringComparison.Ordinal));
                    var ruta = Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                    fichero.SaveAs(ruta);
                    blog.foto = "~/uploads/fotos/" + nombre + extension;
                }

                DbContext.SaveChanges();
            }

            return RedirectToAction("Index", "Tecnicas");
        }

        // action para eliminar una entrada en el blog
        [Authorize]
        public ActionResult Eliminar(int id)
        {
            //buscamos el bicho y lo eliminamos
            if (ModelState.IsValid)
            {
                var blog = DbContext.Tecnica.Find(id);

                //borramos la foto
                var foto = blog.foto;
                var rutacompleta = Server.MapPath("~/") + foto.Substring(2);
                System.IO.File.Delete(rutacompleta);

                DbContext.Tecnica.Remove(blog);
                DbContext.SaveChanges();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}