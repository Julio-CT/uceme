namespace UCEME.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Uceme.Model.Models;
    using Uceme.Model.Models.ClasesVista;
    using UCEME.Seguridad;

    [Authorize]
    public class GestionUsuariosController : SuperController
    {
        [Authorize]
        public ActionResult Index()
        {
            var data = (from o in this.DbContext.Usuario
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
                return this.RedirectToAction("SinPermisos", "GestionUsuarios");
            }
            else
            {
                //sacamos el usuario completo
                var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                var usu = this.DbContext.Usuario.FirstOrDefault(oo => oo.login == cus.Email);

                //Comprobamos si el usuario es superAdmin
                if (usu != null && usu.idRol == 1)
                {
                    return this.View(data);
                }
                else
                {
                    return this.RedirectToAction("SinPermisos", "GestionUsuarios");
                }
            }
        }

        public ActionResult SinPermisos()
        {
            return this.View();
        }

        [Authorize]
        public ActionResult NuevoUsuario()
        {
            this.CargarCombos(null);
            return this.View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult NuevoUsuario(UsuarioVista model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
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

                    this.DbContext.DatosContacto.Add(dc);
                    this.DbContext.SaveChanges();

                    usu.idDatosContacto = dc.idDatosContacto;

                    //Y también necesito un idCurriculum

                    var cv = new Curriculum
                    {
                        Titulo = "Rellenar por el usuario"
                    };

                    this.DbContext.Curriculum.Add(cv);
                    this.DbContext.SaveChanges();

                    usu.idCurriculum = cv.idCurriculum;

                    //Ahora la foto
                    if (fichero != null && fichero.ContentLength > 0)
                    {
                        //guardo previamente el usuario para poder asignar su id a la foto
                        usu.foto = "sin foto";
                        this.DbContext.Usuario.Add(usu);
                        this.DbContext.SaveChanges();

                        var nombreFichero = fichero.FileName;
                        var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.Ordinal));
                        var rutacompleta = this.Server.MapPath("~/uploads/fotos") + @"\usu" + usu.idUsuario + extension;
                        fichero.SaveAs(rutacompleta);
                        usu.foto = "~/uploads/fotos/usu" + usu.idUsuario + extension;

                        this.DbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    this.ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                    return this.RedirectToAction("NuevoUsuario", "GestionUsuarios", model);
                }
            }

            return this.RedirectToAction("Index", "GestionUsuarios");
        }

        [Authorize]
        public ActionResult EditarUsuario(int id)
        {
            var usu = (from o in this.DbContext.Usuario
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
                this.CargarCombos(usu.IdRol);

                return this.View(usu);
            }

            return this.RedirectToAction("Index", "GestionUsuarios");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult EditarUsuario(UsuarioVista model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
            {
                //Buscamos el usuario a modificar...
                var usu = this.DbContext.Usuario.Find(model.IdUsuario);

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
                    var rutacompleta = this.Server.MapPath("~/uploads/fotos") + @"\usu" + model.IdUsuario + extension;
                    fichero.SaveAs(rutacompleta);
                    usu.foto = "~/uploads/fotos/usu" + model.IdUsuario + extension;
                }

                this.DbContext.SaveChanges();
            }

            return this.RedirectToAction("Index", "GestionUsuarios");
        }

        [Authorize]
        public ActionResult BorrarUsuario(int id)
        {
            try
            {
                var usu = this.DbContext.Usuario.Find(id);

                //Primero borro los datos de contacto
                var dc = this.DbContext.DatosContacto.Find(usu.idDatosContacto);
                this.DbContext.DatosContacto.Remove(dc);
                //db.SaveChanges();

                //Luego los datos de su C.V.
                var icv = (from o in this.DbContext.ItemCurriculum
                           where o.idCurriculum == usu.idCurriculum
                           select o).ToList();

                foreach (var i in icv)
                {
                    this.DbContext.ItemCurriculum.Remove(i);
                }
                //db.SaveChanges();

                var cv = this.DbContext.Curriculum.Find(usu.idCurriculum);
                this.DbContext.Curriculum.Remove(cv);
                //db.SaveChanges();

                //Ahora actualizo el idusuario de sus blogs a 6 que es el idUsuario del superAdmin
                var blogs = (from o in this.DbContext.Blog where o.idUsuario == id select o).ToList();

                foreach (Blog t in blogs)
                {
                    t.idUsuario = 6;
                }
                //db.SaveChanges();

                //y ahora actualizo igual sus documentos
                var docus = (from o in this.DbContext.Documento where o.idUsuario == id select o).ToList();

                foreach (Documento t in docus)
                {
                    t.idUsuario = 6;
                }
                //db.SaveChanges();

                //y ya por fin puedo borrar el usuario
                this.DbContext.Usuario.Remove(usu);

                //y hago un SaveChanges pa to así si falla en algún punto no se guarda na no??
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorDeletingItem) + " " + e.Message);
                return this.RedirectToAction("Index", "GestionUsuarios");
            }

            return this.RedirectToAction("Index", "GestionUsuarios");
        }

        [Authorize]
        public void CargarCombos(int? idSelec)
        {
            var listaRoles = new List<Rol>
            {
                new Rol {idRol = 0, nombre = "Selecciona un Rol"}
            };
            //añadimos un elemento que sea el selector y que de paso nos permita quitar el filtro

            var roles = from o in this.DbContext.Rol
                        select o;
            listaRoles.AddRange(roles);

            this.ViewBag.idRol = idSelec == null ? new SelectList(listaRoles, "idRol", "nombre", "idRol") : new SelectList(listaRoles, "idRol", "nombre", idSelec);
        }
    }
}