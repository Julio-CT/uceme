namespace UCEME.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Uceme.Model.Models;
    using Uceme.Model.Models.ClasesVista;
    using UCEME.Seguridad;

    public class PerfilesController : SuperController
    {
        public ActionResult Index(int id)
        {
            //nos traemos el usuario
            var usr = (from o in this.DbContext.Usuario
                       where o.idUsuario == id
                       select o).FirstOrDefault();

            //lo metemos todo
            if (usr != null)
            {
                var data = new MedicoVista
                {
                    IdUsuario = usr.idUsuario,
                    Nombre = usr.nombre,
                    Apellidos = usr.apellidos,
                    Foto = usr.foto,
                    Login = usr.login,
                    IdCurriculum = usr.idCurriculum,
                    IdDatosContacto = usr.idDatosContacto,
                    Telefono = usr.DatosContacto.telefono,
                    Linkedin = usr.linkedin,
                    Curriculum = new Curriculum
                    {
                        Titulo = usr.Curriculum.Titulo
                    }
                };
                //por el lazy loading, metemos el curriculum a manuji
                foreach (var item in usr.Curriculum.ItemCurriculum)
                {
                    data.Curriculum.ItemCurriculum.Add(item);
                }

                this.ViewBag.ShowEditButton = true;
                return this.View(data);
            }

            //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
            return this.Json("error buscando usuario", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Manage()
        {
            //vamos a redirigir a editar el usuario logueado
            //sacamos el usuario completo
            var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
            var usu = this.DbContext.Usuario.FirstOrDefault(oo => oo.login == cus.Email);
            if (usu != null)
            {
                return this.RedirectToAction("Editar", new { id = usu.idUsuario });
            }

            //no tiene los datos adecuados
            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            //solo vamos a dejar editar a un fulano si es el mismo o si es superadmin
            //sacamos el usuario completo
            ////var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
            ////var usr = Db.Usuario.FirstOrDefault(oo => oo.login == cus.Email);
            var usr = this.DbContext.Usuario.FirstOrDefault(oo => oo.idUsuario == id);
            if (usr != null && (usr.idUsuario == id || usr.idRol == 1))
            {
                //lo metemos todo
                var data = new MedicoVista
                {
                    IdUsuario = usr.idUsuario,
                    Nombre = usr.nombre,
                    Apellidos = usr.apellidos,
                    Foto = usr.foto,
                    Login = usr.login,
                    IdCurriculum = usr.idCurriculum,
                    IdDatosContacto = usr.idDatosContacto,
                    Telefono = usr.DatosContacto.telefono,
                    Linkedin = usr.linkedin,
                    Curriculum = new Curriculum
                    {
                        Titulo = usr.Curriculum.Titulo
                    }
                };
                //por el lazy loading, metemos el curriculum a manuji
                if (usr.Curriculum.ItemCurriculum != null)
                {
                    foreach (var item in usr.Curriculum.ItemCurriculum)
                    {
                        data.Curriculum.ItemCurriculum.Add(item);
                    }
                }

                return this.View(data);
            }

            //no tiene permisos
            return this.RedirectToAction("Index", new { id = id });
        }

        //action para actualizar el perfil
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(MedicoVista model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
            {
                //modificamos el curriculum nice & easy
                var usu = this.DbContext.Usuario.Find(model.IdUsuario);
                usu.Curriculum.Titulo = model.Curriculum.Titulo;
                usu.login = model.Login;
                usu.linkedin = model.Linkedin;
                foreach (var item in model.Curriculum.ItemCurriculum)
                {
                    var pos = item.idItem;
                    var firstOrDefault = usu.Curriculum.ItemCurriculum.FirstOrDefault(o => o.idItem == pos);
                    if (firstOrDefault != null)
                    {
                        firstOrDefault.Titulo = item.Titulo;
                        firstOrDefault.Fechas = item.Fechas;
                        firstOrDefault.Texto = item.Texto ?? string.Empty;
                    }
                }

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

                var url = "~/Perfiles/Index/?id=" + model.IdUsuario;
                return this.Redirect(url);
            }

            //no iene los datos adecuados
            return this.RedirectToAction("Index", "Home");
        }

        //action para agregar un itemcurriculum
        [Authorize]
        public ActionResult Agregar(int idCurriculum, string titulo, string fechas, string texto)
        {
            if (this.ModelState.IsValid)
            {
                var curr = this.DbContext.Curriculum.Find(idCurriculum);
                if (curr != null)
                {
                    var item = new ItemCurriculum
                    {
                        idCurriculum = idCurriculum,
                        Titulo = titulo,
                        Texto = texto,
                        Fechas = fechas
                    };
                    this.DbContext.ItemCurriculum.Add(item);
                    this.DbContext.SaveChanges();
                }
            }

            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }

        // action para eliminar un itemcurriculum
        [Authorize]
        public ActionResult Eliminar(int idItem)
        {
            //buscamos el bicho y lo eliminamos
            if (this.ModelState.IsValid)
            {
                var item = this.DbContext.ItemCurriculum.Find(idItem);
                this.DbContext.ItemCurriculum.Remove(item);
                this.DbContext.SaveChanges();
            }
            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}