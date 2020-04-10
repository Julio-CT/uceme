using System.Web.Mvc;
using Uceme.Model.Models.ClasesVista;

namespace UCEME.Controllers
{
    public class ContactoController : SuperController
    {
        //
        // GET: /Contacto/

        public ActionResult Index()
        {
            var contacto = new Contacto();
            ViewBag.email = System.Configuration.ConfigurationManager.AppSettings["email"];
            ViewBag.telefono = System.Configuration.ConfigurationManager.AppSettings["telefono"];
            ViewBag.facebook = System.Configuration.ConfigurationManager.AppSettings["facebook"];
            ViewBag.twitter = System.Configuration.ConfigurationManager.AppSettings["twitter"];
            ViewBag.linkedin = System.Configuration.ConfigurationManager.AppSettings["linkedin"];
            ViewBag.googleplus = System.Configuration.ConfigurationManager.AppSettings["googleplus"];
            ViewBag.instagram = System.Configuration.ConfigurationManager.AppSettings["instagram"];

            return View(contacto);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Enviar(Contacto model)
        {
            if (model != null && ModelState.IsValid)
            {
                var retorno = Utilidades.Notificaciones.EnviarCorreoContacto(model.Email, model.Nombre, model.Texto);
                return View(retorno);
            }
            return View(false);
        }
    }
}