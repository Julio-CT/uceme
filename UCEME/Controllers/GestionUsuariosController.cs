namespace UCEME.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using UCEME.Models;
    using UCEME.Models.ClasesVista;
    using UCEME.Seguridad;

    [Authorize]
    public class GestionUsuariosController : SuperController
    {
        [Authorize]
        public ActionResult Index()
        {
            var data = (from o in DbContext.Usuario
                        select new UsuarioVista
                        {
                            IdUsuario = o.idUsuario,
                            Nombre = o.nombre,
                            Apellidos = o.apellidos,
                            Nick = o.nick,
                            Login = o.login,
                            Foto = o.foto,
                            UltimoUpdate = o.ultimoupdate.Value,
                            IdRol = o.idRol,
                            IdCurriculum = o.idCurriculum,
                            IdDatosContacto = o.idDatosContacto,
                            Password = o.password,
                            Newsletter = o.newsletter,
                            Linkedin = o.linkedin
                        }).ToList();

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("SinPermisos", "GestionUsuarios");
            }
            else
            {
                //sacamos el usuario completo
                var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                var usu = DbContext.Usuario.FirstOrDefault(oo => oo.login == cus.Email);

                //Comprobamos si el usuario es superAdmin
                if (usu != null && usu.idRol == 1)
                {
                    return View(data);
                }
                else
                {
                    return RedirectToAction("SinPermisos", "GestionUsuarios");
                }
            }
        }

        public ActionResult SinPermisos()
        {
            return View();
        }

        [Authorize]
        public ActionResult NuevoUsuario()
        {
            CargarCombos(null);
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult NuevoUsuario(UsuarioVista model, HttpPostedFileBase fichero)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {
                    var usu = new Usuario
                    {
                        nombre = model.Nombre,
                        apellidos = model.Apellidos,
                        nick = model.Nick,
                        login = model.Login,
                        ultimoupdate = System.DateTime.Now,
                        idRol = model.IdRol,
                        password = model.Login,
                        newsletter = model.Newsletter,
                        linkedin = model.Linkedin
                    };
                    //Le pongo como password el login de momento y ojo sin cifrar!!

                    //Necesito guardar previamente los datos de contacto
                    var dc = new DatosContacto
                    {
                        email = model.DatosContacto.email,
                        direccion = model.DatosContacto.direccion,
                        telefono = model.DatosContacto.telefono
                    };

                    DbContext.DatosContacto.Add(dc);
                    DbContext.SaveChanges();

                    usu.idDatosContacto = dc.idDatosContacto;

                    //Y también necesito un idCurriculum

                    var cv = new Curriculum
                    {
                        Titulo = "Rellenar por el usuario"
                    };

                    DbContext.Curriculum.Add(cv);
                    DbContext.SaveChanges();

                    usu.idCurriculum = cv.idCurriculum;

                    //Ahora la foto
                    if (fichero != null && fichero.ContentLength > 0)
                    {
                        //guardo previamente el usuario para poder asignar su id a la foto
                        usu.foto = "sin foto";
                        DbContext.Usuario.Add(usu);
                        DbContext.SaveChanges();

                        var nombreFichero = fichero.FileName;
                        var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.Ordinal));
                        var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\usu" + usu.idUsuario + extension;
                        fichero.SaveAs(rutacompleta);
                        usu.foto = "~/uploads/fotos/usu" + usu.idUsuario + extension;

                        DbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                    return RedirectToAction("NuevoUsuario", "GestionUsuarios", model);
                }
            }

            return RedirectToAction("Index", "GestionUsuarios");
        }

        [Authorize]
        public ActionResult EditarUsuario(int id)
        {
            var usu = (from o in DbContext.Usuario
                       where o.idUsuario == id
                       select new UsuarioVista
                       {
                           IdUsuario = o.idUsuario,
                           Nombre = o.nombre,
                           Apellidos = o.apellidos,
                           Nick = o.nick,
                           Login = o.login,
                           Foto = o.foto,
                           UltimoUpdate = o.ultimoupdate.Value,
                           IdRol = o.idRol,
                           IdCurriculum = o.idCurriculum,
                           IdDatosContacto = o.idDatosContacto,
                           Password = o.password,
                           Newsletter = o.newsletter,
                           Linkedin = o.linkedin
                       }).FirstOrDefault();
            if (usu != null)
            {
                CargarCombos(usu.IdRol);

                return View(usu);
            }

            return RedirectToAction("Index", "GestionUsuarios");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult EditarUsuario(UsuarioVista model, HttpPostedFileBase fichero)
        {
            if (model != null && ModelState.IsValid)
            {
                //Buscamos el usuario a modificar...
                var usu = DbContext.Usuario.Find(model.IdUsuario);

                usu.nombre = model.Nombre;
                usu.apellidos = model.Apellidos;
                usu.nick = model.Nick;
                usu.login = model.Login;
                usu.idRol = model.IdRol;
                usu.newsletter = model.Newsletter;
                usu.linkedin = model.Linkedin;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos el fichero de la foto con nombre usu + id
                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.Ordinal));
                    var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\usu" + model.IdUsuario + extension;
                    fichero.SaveAs(rutacompleta);
                    usu.foto = "~/uploads/fotos/usu" + model.IdUsuario + extension;
                }

                DbContext.SaveChanges();
            }

            return RedirectToAction("Index", "GestionUsuarios");
        }

        [Authorize]
        public ActionResult BorrarUsuario(int id)
        {
            try
            {
                var usu = DbContext.Usuario.Find(id);

                //Primero borro los datos de contacto
                var dc = DbContext.DatosContacto.Find(usu.idDatosContacto);
                DbContext.DatosContacto.Remove(dc);
                //db.SaveChanges();

                //Luego los datos de su C.V.
                var icv = (from o in DbContext.ItemCurriculum
                           where o.idCurriculum == usu.idCurriculum
                           select o).ToList();

                foreach (var i in icv)
                {
                    DbContext.ItemCurriculum.Remove(i);
                }
                //db.SaveChanges();

                var cv = DbContext.Curriculum.Find(usu.idCurriculum);
                DbContext.Curriculum.Remove(cv);
                //db.SaveChanges();

                //Ahora actualizo el idusuario de sus blogs a 6 que es el idUsuario del superAdmin
                var blogs = (from o in DbContext.Blog where o.idUsuario == id select o).ToList();

                foreach (Blog t in blogs)
                {
                    t.idUsuario = 6;
                }
                //db.SaveChanges();

                //y ahora actualizo igual sus documentos
                var docus = (from o in DbContext.Documento where o.idUsuario == id select o).ToList();

                foreach (Documento t in docus)
                {
                    t.idUsuario = 6;
                }
                //db.SaveChanges();

                //y ya por fin puedo borrar el usuario
                DbContext.Usuario.Remove(usu);

                //y hago un SaveChanges pa to así si falla en algún punto no se guarda na no??
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorDeletingItem) + " " + e.Message);
                return RedirectToAction("Index", "GestionUsuarios");
            }

            return RedirectToAction("Index", "GestionUsuarios");
        }

        [Authorize]
        public void CargarCombos(int? idSelec)
        {
            var listaRoles = new List<Rol>
            {
                new Rol {idRol = 0, nombre = "Selecciona un Rol"}
            };
            //añadimos un elemento que sea el selector y que de paso nos permita quitar el filtro

            var roles = from o in DbContext.Rol
                        select o;
            listaRoles.AddRange(roles);

            ViewBag.idRol = idSelec == null ? new SelectList(listaRoles, "idRol", "nombre", "idRol") : new SelectList(listaRoles, "idRol", "nombre", idSelec);
        }
    }
}