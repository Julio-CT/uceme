using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCEME.Models;
using UCEME.Models.ClasesVista;
using UCEME.Seguridad;

namespace UCEME.Controllers
{
    public class ArticulosController : SuperController
    {
        private static List<BlogVista> _conjuntodata;

        private static readonly int Elementospp = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["numeroelementos"]);

        public ArticulosController()
        {
            _conjuntodata = (from o in DbContext.Blog
                             orderby o.fecha descending
                             select new BlogVista
                             {
                                 IdBlog = o.idBlog,
                                 Usuario = o.Usuario.nombre + " " + o.Usuario.apellidos,
                                 Titulo = o.titulo,
                                 Fecha = o.fecha,
                                 Foto = o.foto,
                                 Texto = o.texto,
                                 Ano = o.fecha.Year,
                                 Mes = o.fecha.Month,
                                 Dia = o.fecha.Day
                             }).ToList();
        }

        //
        // GET: /Articulos/
        public ActionResult Index(int? id)
        {
            //scrolling
            System.Threading.Thread.Sleep(2000);

            var pagina = id ?? 0;

            var data = GetSubconjunto(pagina);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Subconjunto", data);
            }

            return View(data);
        }

        //nuevo!! para traer un solo articulo (para cuando linkemos desde fb o tw)
        public ActionResult Uno(int id)
        {
            var data = (from o in DbContext.Blog
                        where o.idBlog == id
                        select new BlogVista
                        {
                            IdBlog = o.idBlog,
                            Usuario = o.Usuario.nombre + " " + o.Usuario.apellidos,
                            Titulo = o.titulo,
                            Fecha = o.fecha,
                            Foto = o.foto,
                            Texto = o.texto,
                            Ano = o.fecha.Year,
                            Mes = o.fecha.Month,
                            Dia = o.fecha.Day
                        }).ToList();

            return View("Index", data);
        }

        [Authorize]
        public ActionResult Anadir()
        {
            var blog = new BlogVista();
            return View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Anadir(BlogVista model, HttpPostedFileBase fichero)
        {
            if (model != null && ModelState.IsValid)
            {
                if (fichero == null || fichero.ContentLength <= 0) return RedirectToAction("Index", "Articulos");
                var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                var usu = DbContext.Usuario.FirstOrDefault(oo => oo.login == cus.Email);

                if (usu == null) return RedirectToAction("Index", "Articulos");
                var blog = new Blog
                {
                    idUsuario = usu.idUsuario,
                    titulo = model.Titulo,
                    fecha = DateTime.Now,
                    texto = model.Texto,
                    foto = "",
                    profesional = true
                };

                DbContext.Blog.Add(blog);
                DbContext.SaveChanges();

                try
                {
                    var nombre = "Articulo" + blog.idBlog;
                    var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", comparisonType: StringComparison.Ordinal));
                    var ruta = Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                    fichero.SaveAs(ruta);
                    blog.foto = "~/uploads/fotos/" + nombre + extension;

                    DbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                    DbContext.Blog.Remove(blog);
                    DbContext.SaveChanges();

                    ModelState.AddModelError("", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Articulos");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var blog = (from o in DbContext.Blog
                        where o.idBlog == id
                        select new BlogVista
                        {
                            IdBlog = o.idBlog,
                            Usuario = o.Usuario.nombre + " " + o.Usuario.apellidos,
                            Titulo = o.titulo,
                            Fecha = o.fecha,
                            Foto = o.foto,
                            Profesional = true,
                            Texto = o.texto
                        }).FirstOrDefault();

            return View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(BlogVista model, HttpPostedFileBase fichero)
        {
            if (model != null && ModelState.IsValid)
            {
                /* Se supone que un usuario solo podrá editar sus blog asi que dejo el usuario sin modificar
                 CustomIdentity cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                 Usuario usu = db.Usuario.FirstOrDefault(oo => oo.login == cus.Email);
                 */

                //Buscamos el blog a modificar...
                var blog = DbContext.Blog.Find(model.IdBlog);

                blog.titulo = model.Titulo;
                blog.texto = model.Texto;
                blog.profesional = true;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos la nueva imagen con la misma ruta que tenía antes, solo cambia el nombre
                    var nombre = "Articulo" + model.IdBlog;
                    var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", comparisonType: StringComparison.Ordinal));
                    var ruta = Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                    fichero.SaveAs(ruta);
                    blog.foto = "~/uploads/fotos/" + nombre + extension;
                }

                DbContext.SaveChanges();
            }

            return RedirectToAction("Index", "Articulos");
        }

        // action para eliminar una entrada en el blog
        [Authorize]
        public ActionResult Eliminar(int id)
        {
            //buscamos el bicho y lo eliminamos
            if (ModelState.IsValid)
            {
                var blog = DbContext.Blog.Find(id);

                //borramos la foto
                var foto = blog.foto;
                var rutacompleta = Server.MapPath("~/") + foto.Substring(2);
                System.IO.File.Delete(rutacompleta);

                DbContext.Blog.Remove(blog);
                DbContext.SaveChanges();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        private static List<BlogVista> GetSubconjunto(int pagina = 1)
        {
            var skipRecords = pagina * Elementospp;

            return _conjuntodata.
                Skip(skipRecords).
                Take(Elementospp).ToList();
        }
    }
}