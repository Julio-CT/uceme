using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using TweetSharp;
using UCEME.Models;
using UCEME.Models.ClasesVista;
using UCEME.Seguridad;

namespace UCEME.Controllers
{
    public class BlogController : SuperController
    {
        private static List<BlogVista> _conjuntodata;
        private static readonly int Elementospp = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["numeroelementos"]);

        public BlogController()
        {
            _conjuntodata = (from o in DbContext.Blog
                             orderby o.fecha descending
                             select new BlogVista
                             {
                                 IdBlog = o.idBlog,
                                 Usuario = o.Usuario.nombre + " " + o.Usuario.apellidos,
                                 Titulo = o.titulo,
                                 Fecha = o.fecha,
                                 Foto = o.foto,
                                 Texto = o.texto,
                                 Ano = o.fecha.Year,
                                 Mes = o.fecha.Month,
                                 Dia = o.fecha.Day
                             }).ToList();
        }

        private List<BlogVista> GetSubconjunto(int pagina = 1)
        {
            var skipRecords = pagina * Elementospp;

            return _conjuntodata.
                Skip(skipRecords).
                Take(Elementospp).ToList();
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

        //nuevo!! para traer un solo articulo (para cuando linkemos desde fb o tw)
        public ActionResult Uno(int id)
        {
            var data = (from o in DbContext.Blog
                        where o.idBlog == id
                        select new BlogVista
                        {
                            IdBlog = o.idBlog,
                            Usuario = o.Usuario.nombre + " " + o.Usuario.apellidos,
                            Titulo = o.titulo,
                            Fecha = o.fecha,
                            Foto = o.foto,
                            Texto = o.texto,
                            Ano = o.fecha.Year,
                            Mes = o.fecha.Month,
                            Dia = o.fecha.Day
                        }).ToList();

            return View("Index", data);
        }

        [Authorize]
        public ActionResult Anadir()
        {
            var blog = new BlogVista();
            return View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Anadir(BlogVista model, HttpPostedFileBase fichero)
        {
            if (model != null && ModelState.IsValid)
            {
                if (fichero != null && fichero.ContentLength > 0)
                {
                    var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                    var usu = DbContext.Usuario.FirstOrDefault(oo => oo.login == cus.Email);

                    var blog = new Blog();

                    if (usu != null) blog.idUsuario = usu.idUsuario;
                    blog.titulo = model.Titulo;
                    blog.fecha = model.Fecha;
                    blog.texto = model.Texto;
                    blog.foto = "";
                    blog.profesional = false;
                    DbContext.Blog.Add(blog);
                    DbContext.SaveChanges();

                    try
                    {
                        var nombre = "Blog" + blog.idBlog;
                        var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", StringComparison.Ordinal));
                        var ruta = Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                        fichero.SaveAs(ruta);
                        blog.foto = "~/uploads/fotos/" + nombre + extension;

                        DbContext.SaveChanges();

                        PublicarEnRedesSociales(blog, nombre, extension);
                    }
                    catch (Exception e)
                    {
                        //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                        DbContext.Blog.Remove(blog);
                        DbContext.SaveChanges();

                        ModelState.AddModelError("", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                        return View(model);
                    }
                }
            }

            return RedirectToAction("Index", "Blog");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var blog = (from o in DbContext.Blog
                        where o.idBlog == id
                        select new BlogVista
                        {
                            IdBlog = o.idBlog,
                            Usuario = o.Usuario.nombre + " " + o.Usuario.apellidos,
                            Titulo = o.titulo,
                            Fecha = o.fecha,
                            Foto = o.foto,
                            Profesional = false,
                            Texto = o.texto
                        }).FirstOrDefault();
            return View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(BlogVista model, HttpPostedFileBase fichero)
        {
            if (model != null && ModelState.IsValid)
            {
                /* Se supone que un usuario solo podrá editar sus blog asi que dejo el usuario sin modificar
                 CustomIdentity cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                 Usuario usu = db.Usuario.FirstOrDefault(oo => oo.login == cus.Email);
                 */

                //Buscamos el blog a modificar...
                var blog = DbContext.Blog.Find(model.IdBlog);

                blog.titulo = model.Titulo;
                blog.texto = model.Texto;
                blog.profesional = false;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos la nueva imagen con la misma ruta que tenía antes, solo cambia el nombre
                    var nombre = string.Format("Blog{0}", model.IdBlog);
                    var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", comparisonType: StringComparison.Ordinal));
                    var ruta = Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                    fichero.SaveAs(ruta);
                    blog.foto = "~/uploads/fotos/" + nombre + extension;
                }

                DbContext.SaveChanges();
            }
            return RedirectToAction("Index", "Blog");
        }

        // action para eliminar una entrada en el blog
        [Authorize]
        public ActionResult Eliminar(int id)
        {
            //buscamos el bicho y lo eliminamos
            if (ModelState.IsValid)
            {
                var blog = DbContext.Blog.Find(id);

                //borramos la foto
                var foto = blog.foto;
                var rutacompleta = Server.MapPath("~/") + foto.Substring(2);
                System.IO.File.Delete(rutacompleta);

                DbContext.Blog.Remove(blog);
                DbContext.SaveChanges();
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        private void PublicarEnRedesSociales(Blog blog, string nombre, string extension)
        {
            //la direccion del nuevo post es:
            var direccionnueva = System.Configuration.ConfigurationManager.AppSettings["url_actual"] + "/blog/uno/?id=" + blog.idBlog;
            string direccioncorta = null;
            Stream stream = null;
            Stream responseStream;
            // vamos a meter todo lo de las peticiones en un try distinto, por si acaso
            try
            {
                //
                //acortamos la direccion usando goo.gl
                //
                var httpWebRequest =
                    (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                stream = httpWebRequest.GetRequestStream();
                using (var streamWriter = new StreamWriter(stream))
                {
                    var json = "{\"longUrl\":\"" + direccionnueva + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (var httpResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    var jsonSerializer = new DataContractJsonSerializer(typeof(RespuestaGoogle));
                    if (httpResponse != null)
                    {
                        responseStream = httpResponse.GetResponseStream();
                        if (responseStream == null)
                        {
                            throw new NoNullAllowedException("jsonResponse is null");
                        }

                        var objResponse = jsonSerializer.ReadObject(stream: responseStream);
                        var jsonResponse = objResponse as RespuestaGoogle;
                        if (jsonResponse == null)
                        {
                            throw new NoNullAllowedException("jsonResponse is null");
                        }

                        direccioncorta = jsonResponse.Id;
                    }
                }

                //
                //publicar en tw
                //
                var tweet = string.Format("{0} {1}", blog.titulo, direccioncorta);

                //nos basamos en el las librerias de TwitterSharp de nuget
                var service = new TwitterService(System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"],
                    System.Configuration.ConfigurationManager.AppSettings["ConsumerSecret"]);
                service.AuthenticateWith(System.Configuration.ConfigurationManager.AppSettings["AccessToken"],
                    System.Configuration.ConfigurationManager.AppSettings["AccessTokenSecret"]);
                service.SendTweet(new SendTweetOptions { Status = tweet });

                //
                //publicar en fb
                //
                var direccionfoto = System.Configuration.ConfigurationManager.AppSettings["url_actual"] +
                                    "/uploads/fotos/" +
                                    nombre + extension;

                httpWebRequest =
                    (HttpWebRequest)
                        WebRequest.Create("https://graph.facebook.com/" +
                                          System.Configuration.ConfigurationManager.AppSettings["AppId"] + "/feed");

                var url = "https://graph.facebook.com/" + System.Configuration.ConfigurationManager.AppSettings["AppId"] +
                          "/feed?access_token=" + System.Configuration.ConfigurationManager.AppSettings["Token"];

                var parameters = "link=" + direccioncorta + "&picture=" + direccionfoto + "&description=\"" +
                                 blog.titulo + "\"";

                // es un POST con URLENCODED...asi que es distinto
                var webRequest = WebRequest.Create(url);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";

                var bytes = System.Text.Encoding.ASCII.GetBytes(parameters);
                webRequest.ContentLength = bytes.Length;

                var os = webRequest.GetRequestStream();

                os.Write(bytes, 0, bytes.Length);
                os.Close();

                var webResponse = webRequest.GetResponse();
                responseStream = webResponse.GetResponseStream();
                if (responseStream != null)
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        var postId = sr.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                // pillamos la excepcion con todos los detalles
                responseStream = ex.Response.GetResponseStream();
                if (responseStream != null)
                {
                    string errorMessage;
                    using (var errorStream = new StreamReader(responseStream))
                    {
                        errorMessage = errorStream.ReadToEnd();
                    }
                    ModelState.AddModelError("",
                        Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorPublishingToSocialNetwork) +
                        " " + ex.Message + "" + errorMessage);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
    }

    [DataContract]
    internal class RespuestaGoogle
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "longUrl")]
        public string LongUrl { get; set; }
    }
}