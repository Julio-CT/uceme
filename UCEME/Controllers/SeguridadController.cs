using System;
using System.Linq;
using System.Web.Mvc;
using Uceme.Model.Models;
using Uceme.Model.Models.ClasesVista;
using UCEME.Seguridad;
using UCEME.Utilidades;

namespace UCEME.Controllers
{
    public class SeguridadController : Controller
    {
        //
        // GET: /Seguridad/

        public ActionResult Login()
        {
            var usu = new Usuario();
            return this.View(usu);
        }

        [HttpPost]
        public ActionResult Login(Usuario model, string returnUrl)
        {
            if (model != null)
            {
                //probamos si el usuario y la password son validos
                if (CustomPrincipal.Login(model.login, model.password, model.Recordar))
                {
                    //si son validos, retornamos a la pagina de la que viniesemos
                    if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/", StringComparison.CurrentCulture) && !returnUrl.StartsWith("//", StringComparison.CurrentCulture) && !returnUrl.StartsWith("/\\", StringComparison.CurrentCulture))
                    {
                        return this.Redirect(returnUrl);
                    }

                    ////var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;

                    //distinguimos entre un tipo de usuario y otro y redirigimos
                    if (System.Web.HttpContext.Current.User.IsInRole("SuperAdmin"))
                    {
                        return this.RedirectToAction("Index", "GestionUsuarios");
                    }

                    return this.RedirectToAction("Index", "home");
                }
                //si no son validos, datos incorrectos
                this.ModelState.AddModelError("", "Datos incorrectos");
            }
            return this.View(model);
        }

        public ActionResult Logout()
        {
            CustomPrincipal.LogOut();
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EmailEnviado()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult ErrorRecovery()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            this.ViewBag.error = "";
            return this.View();
        }

        [HttpPost]
        public ActionResult PasswordRecovery(Usuario model)
        {
            if (this.ModelState.IsValid)
            {
                var username = CustomIdentity.GetIdUsuario(model.login);

                if (username != -1)
                {
                    //recuperamos el usuario
                    var usu = ConsultasBbdd.GetUsuariobyId(username);

                    // enviamos el email con la url magica..
                    var token = Encodificacion.EncodeMessageWithPassword(usu.login, usu.password);
                    var retorno = Notificaciones.SendPasswordRetrieval(model.login, token);
                    if (!retorno)
                    {
                        this.ViewBag.error = "ErrorEnviandoNotification";
                        return this.View();
                    }

                    return this.RedirectToAction("EmailEnviado", "Seguridad");
                }

                System.Diagnostics.Trace.WriteLine(string.Format("*** WARNING:  A user tried to retrieve their password but the email address used '{0}' does not exist in the database.", model.login));
                this.ViewBag.error = "error";
                return this.View();
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Validate(string email, string token)
        {
            var username = CustomIdentity.GetIdUsuario(email);

            if (username != -1)
            {
                //recuperamos el usuario
                var usu = ConsultasBbdd.GetUsuariobyId(username);

                //decodificamos
                var emaildeco = Encodificacion.DecodeMessageWithPassword(token, usu.password);
                //comprobamos que coincide la deco con la de la base de datos
                if (emaildeco == usu.login)
                {
                    //redirigimos a la vista de recuperar password
                    var pass = new CambioPasswordVista { UsuarioActual = usu.login };

                    return this.View(pass);
                }
                //las passwords no coinciden
                return this.RedirectToAction("ErrorRecovery", "Seguridad");
            }
            //esta mal el email
            return this.RedirectToAction("ErrorRecovery", "Seguridad");
        }

        [HttpPost]
        public ActionResult Validate(CambioPasswordVista model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                //recuperamos el usuario y guardamos la password nueva
                var username = CustomIdentity.GetIdUsuario(model.UsuarioActual);
                using (var db = new UCEMEDbEntities())
                {
                    var usu = db.Usuario.FirstOrDefault(o => o.idUsuario == username);
                    var passwencriptada = Encodificacion.GetSha1(model.PasswordNueva);
                    if (usu != null)
                    {
                        usu.password = passwencriptada;
                    }
                    else
                    {
                        //las passwords no coindiden
                        this.ViewBag.error = "UsuarioNoEncontrado";
                        return this.View(model);
                    }
                    //usu.password = model.PasswordNueva;
                    db.SaveChanges();
                }
            }
            else
            {
                //las passwords no coindiden
                this.ViewBag.error = "PasswordsNoCoinciden";
                return this.View(model);
            }

            return this.RedirectToAction("Login", "Seguridad");
        }
    }
}