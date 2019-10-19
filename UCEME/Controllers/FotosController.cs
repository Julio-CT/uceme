using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCEME.Models;
using UCEME.Models.ClasesVista;

namespace UCEME.Controllers
{
    public class FotosController : SuperController
    {
        private static List<FotosVista> _conjuntodata;

        private static int _elementospp = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["numerofotos"]);

        public FotosController()
        {
            _conjuntodata = (from o in DbContext.Fotos
                             orderby o.posicion
                             select new FotosVista
                             {
                                 IdFoto = o.idFoto,
                                 Nombre = o.nombre,
                                 Texto = o.texto,
                                 Destacada = o.destacada,
                                 Posicion = o.posicion
                             }).ToList();

            var listaPosiciones = new List<int>();

            foreach (var d in _conjuntodata)
            {
                if (d.Posicion != null)
                {
                    listaPosiciones.Add(d.Posicion.Value);
                }
            }

            ViewBag.posiciones = listaPosiciones;
        }

        private List<FotosVista> GetSubconjunto(int pagina = 1)
        {
            var skipRecords = pagina * _elementospp;

            return _conjuntodata.
                Skip(skipRecords).
                Take(_elementospp).ToList();
        }

        public ActionResult Index(int? id)
        {
            //scrolling
            System.Threading.Thread.Sleep(2000);

            var pagina = id ?? 0;

            var data = GetSubconjunto(pagina);

            if (Request.IsAjaxRequest())
                return PartialView("Subconjunto", data);

            return View(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Subir(FotosVista model, HttpPostedFileBase fichero, FormCollection collection)
        {
            if (model != null && ModelState.IsValid)
            {
                if (fichero != null && fichero.ContentLength > 0)
                {
                    var f = new Fotos
                    {
                        nombre = "lo pongo a posteriori",
                        texto = collection.Get(0),
                        destacada = false
                    };

                    var ultPos = (from o in DbContext.Fotos orderby o.posicion descending select o.posicion).FirstOrDefault();
                    f.posicion = ultPos + 1;

                    DbContext.Fotos.Add(f);
                    DbContext.SaveChanges();

                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.CurrentCulture));
                    var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\img" + f.idFoto + extension;
                    fichero.SaveAs(rutacompleta);
                    f.nombre = "~/uploads/fotos/img" + f.idFoto + extension;

                    DbContext.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Fotos");
        }

        public ActionResult CambiarDestacada(string id)
        {
            var idFoto = Convert.ToInt32(id);
            var f = DbContext.Fotos.Find(idFoto);
            f.destacada = f.destacada != true;

            DbContext.SaveChanges();

            var destacada = f.destacada;
            return Json(destacada, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GuardarCambios(int id, int pos, string titulo)
        {
            if (ModelState.IsValid)
            {
                var foto = DbContext.Fotos.Find(id);
                if (foto.posicion != null)
                {
                    var posAntes = foto.posicion.Value;

                    if (posAntes > pos)
                    {
                        var fotosAntes = (from o in DbContext.Fotos
                                          where o.posicion >= pos && o.posicion < posAntes
                                          orderby o.posicion
                                          select o).ToList();
                        foreach (var f in fotosAntes)
                        {
                            f.posicion = f.posicion + 1;
                        }
                        foto.posicion = pos;
                    }
                    else
                    {
                        if (posAntes < pos)
                        {
                            var fotosDespues = (from o in DbContext.Fotos
                                                where o.posicion > posAntes && o.posicion <= pos
                                                orderby o.posicion
                                                select o).ToList();
                            foreach (var f in fotosDespues)
                            {
                                f.posicion = f.posicion - 1;
                            }
                            foto.posicion = pos;
                        }
                    }

                    foto.texto = titulo;
                    DbContext.SaveChanges();
                }
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            //buscamos el bicho y lo eliminamos
            if (ModelState.IsValid)
            {
                var foto = DbContext.Fotos.Find(id);
                if (foto.posicion != null)
                {
                    var pos = foto.posicion.Value;
                    var ultPos = (from o in DbContext.Fotos orderby o.posicion descending select o.posicion).FirstOrDefault();

                    if (pos < ultPos)
                    {
                        var fotosDespues =
                            (from o in DbContext.Fotos where o.posicion > pos && o.posicion <= ultPos orderby o.posicion select o).
                                ToList();
                        foreach (var f in fotosDespues)
                        {
                            f.posicion = f.posicion - 1;
                        }
                    }

                    DbContext.Fotos.Remove(foto);
                    DbContext.SaveChanges();
                }
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}