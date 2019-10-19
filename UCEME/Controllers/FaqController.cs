using System.Collections.Generic;
using System.Web.Mvc;
using UCEME.Models;

namespace UCEME.Controllers
{
    public class FaqController : SuperController
    {
        //
        // GET: /Faq/

        public ActionResult Index()
        {
            var data = DbContext.Faq;
            return View(data);
        }

        [Authorize]
        public ActionResult Editar()
        {
            var data = DbContext.Faq;
            return View(data);
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
                    var bicho = DbContext.Faq.Find(t.idFaq);
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
                    DbContext.Faq.Add(bicho);
                }
                DbContext.SaveChanges();
            }

            return RedirectToAction("index");
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            if (ModelState.IsValid)
            {
                var faqItem = DbContext.Faq.Find(id);
                DbContext.Faq.Remove(faqItem);
                DbContext.SaveChanges();
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}