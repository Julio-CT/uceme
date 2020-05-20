namespace UCEME.Controllers
{
    using System.Web.Mvc;
    using Uceme.Model.Models.ClasesVista;

    public class ContactoController : SuperController
    {
        //
        // GET: /Contacto/

        public ActionResult Index()
        {
            var contacto = new Contacto();
            this.ViewBag.email = System.Configuration.ConfigurationManager.AppSettings["email"];
            this.ViewBag.telefono = System.Configuration.ConfigurationManager.AppSettings["telefono"];
            this.ViewBag.facebook = System.Configuration.ConfigurationManager.AppSettings["facebook"];
            this.ViewBag.twitter = System.Configuration.ConfigurationManager.AppSettings["twitter"];
            this.ViewBag.linkedin = System.Configuration.ConfigurationManager.AppSettings["linkedin"];
            this.ViewBag.googleplus = System.Configuration.ConfigurationManager.AppSettings["googleplus"];
            this.ViewBag.instagram = System.Configuration.ConfigurationManager.AppSettings["instagram"];

            return this.View(contacto);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Enviar(Contacto model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var retorno = Utilidades.Notificaciones.EnviarCorreoContacto(model.Email, model.Nombre, model.Texto);
                return this.View(retorno);
            }
            return this.View(false);
        }
    }
}