using System.Collections.Generic;
using System.Web.Mvc;
using Uceme.Model.Models;

namespace UCEME.Controllers
{
    public class FaqController : SuperController
    {
        //
        // GET: /Faq/

        public ActionResult Index()
        {
            var data = this.DbContext.Faq;
            return this.View(data);
        }

        [Authorize]
        public ActionResult Editar()
        {
            var data = this.DbContext.Faq;
            return this.View(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(List<Faq> model)
        {
            foreach (var t in model)
            {
                //los que tengan el id a null son los que hemos añadido..
                //asi que unos son update y los otros insert
                if (t.idFaq != 0)
                {
                    var bicho = this.DbContext.Faq.Find(t.idFaq);
                    bicho.texto = t.texto;
                    bicho.titulo = t.titulo;
                }
                else
                {
                    var bicho = new Faq
                    {
                        texto = t.texto,
                        titulo = t.titulo
                    };
                    this.DbContext.Faq.Add(bicho);
                }
                this.DbContext.SaveChanges();
            }

            return this.RedirectToAction("index");
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            if (this.ModelState.IsValid)
            {
                var faqItem = this.DbContext.Faq.Find(id);
                this.DbContext.Faq.Remove(faqItem);
                this.DbContext.SaveChanges();
            }
            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}