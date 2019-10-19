using System;
using System.Linq;
using System.Web.Mvc;
using UCEME.Models;
using UCEME.Models.ClasesVista;
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
            return View(usu);
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
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/", StringComparison.CurrentCulture) && !returnUrl.StartsWith("//", StringComparison.CurrentCulture) && !returnUrl.StartsWith("/\\", StringComparison.CurrentCulture))
                    {
                        return Redirect(returnUrl);
                    }

                    ////var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;

                    //distinguimos entre un tipo de usuario y otro y redirigimos
                    if (System.Web.HttpContext.Current.User.IsInRole("SuperAdmin"))
                    {
                        return RedirectToAction("Index", "GestionUsuarios");
                    }

                    return RedirectToAction("Index", "home");
                }
                //si no son validos, datos incorrectos
                ModelState.AddModelError("", "Datos incorrectos");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            CustomPrincipal.LogOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EmailEnviado()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ErrorRecovery()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            ViewBag.error = "";
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRecovery(Usuario model)
        {
            if (ModelState.IsValid)
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
                        ViewBag.error = "ErrorEnviandoNotification";
                        return View();
                    }

                    return RedirectToAction("EmailEnviado", "Seguridad");
                }

                System.Diagnostics.Trace.WriteLine(String.Format("*** WARNING:  A user tried to retrieve their password but the email address used '{0}' does not exist in the database.", model.login));
                ViewBag.error = "error";
                return View();
            }

            return View(model);
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

                    return View(pass);
                }
                //las passwords no coinciden
                return RedirectToAction("ErrorRecovery", "Seguridad");
            }
            //esta mal el email
            return RedirectToAction("ErrorRecovery", "Seguridad");
        }

        [HttpPost]
        public ActionResult Validate(CambioPasswordVista model)
        {
            if (model != null && ModelState.IsValid)
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
                        ViewBag.error = "UsuarioNoEncontrado";
                        return View(model);
                    }
                    //usu.password = model.PasswordNueva;
                    db.SaveChanges();
                }
            }
            else
            {
                //las passwords no coindiden
                ViewBag.error = "PasswordsNoCoinciden";
                return View(model);
            }

            return RedirectToAction("Login", "Seguridad");
        }
    }
}