﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UCEME.Models;
using UCEME.Models.ClasesVista;

namespace UCEME.Controllers
{
    public class VideosController : SuperController
    {
        //
        // GET: /Videos/
        private static List<VideosVista> _conjuntodata;

        private static readonly int _elementospp = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["numerovideos"]);

        public VideosController()
        {
            _conjuntodata = (from o in DbContext.Video
                             orderby o.posicion
                             select new VideosVista
                             {
                                 IdVideo = o.idVideo,
                                 Titulo = o.titulo,
                                 Descripcion = o.descripcion,
                                 Link = o.link,
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

        private List<VideosVista> GetSubconjunto(int pagina = 1)
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
        public ActionResult Subir(FotosVista model, string nuevoLink, string nuevoTitulo, string nuevaDescripcion)
        {
            if (model != null && ModelState.IsValid)
            {
                var v = new Video
                {
                    link = nuevoLink,
                    titulo = nuevoTitulo,
                    descripcion = nuevaDescripcion
                };

                var ultPos = (from o in DbContext.Video orderby o.posicion descending select o.posicion).FirstOrDefault();
                v.posicion = ultPos + 1;

                DbContext.Video.Add(v);
                DbContext.SaveChanges();
            }
            return RedirectToAction("Index", "Videos");
        }

        [Authorize]
        public ActionResult GuardarCambios(int id, int pos, string titulo, string descripcion)
        {
            if (ModelState.IsValid)
            {
                var video = DbContext.Video.Find(id);
                if (video.posicion != null)
                {
                    var posAntes = video.posicion.Value;

                    if (posAntes > pos)
                    {
                        var videosAntes = (from o in DbContext.Video
                                           where o.posicion >= pos && o.posicion < posAntes
                                           orderby o.posicion
                                           select o).ToList();
                        foreach (var f in videosAntes)
                        {
                            f.posicion += 1;
                        }
                        video.posicion = pos;
                    }
                    else
                    {
                        if (posAntes < pos)
                        {
                            var videosDespues = (from o in DbContext.Video
                                                 where o.posicion > posAntes && o.posicion <= pos
                                                 orderby o.posicion
                                                 select o).ToList();
                            foreach (var f in videosDespues)
                            {
                                f.posicion -= 1;
                            }
                            video.posicion = pos;
                        }
                    }
                    video.titulo = titulo;
                    video.descripcion = descripcion;
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
                var video = DbContext.Video.Find(id);
                if (video.posicion != null)
                {
                    var pos = video.posicion.Value;
                    var ultPos = (from o in DbContext.Video orderby o.posicion descending select o.posicion).FirstOrDefault();

                    if (pos < ultPos)
                    {
                        var videosDespues =
                            (from o in DbContext.Video where o.posicion > pos && o.posicion <= ultPos orderby o.posicion select o).
                                ToList();
                        foreach (var f in videosDespues)
                        {
                            f.posicion -= 1;
                        }
                    }

                    DbContext.Video.Remove(video);
                    DbContext.SaveChanges();
                }
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}