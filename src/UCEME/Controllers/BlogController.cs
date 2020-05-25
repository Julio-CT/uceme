namespace UCEME.Controllers
{
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
    using Uceme.Model.Models;
    using Uceme.Model.Models.ClasesVista;
    using UCEME.Seguridad;

    public class BlogController : SuperController
    {
        private static List<BlogVista> _conjuntodata;
        private static readonly int Elementospp = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["numeroelementos"]);

        public BlogController()
        {
            if (this.User != null && this.User.Identity != null && this.User.Identity.IsAuthenticated)
            {
                _conjuntodata = (from o in this.DbContext.Blog
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
            else
            {
                _conjuntodata = (from o in this.DbContext.Blog
                                 where o.fecha <= DateTime.Today
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
        }

        public ActionResult Index(int? id)
        {
            var pagina = id ?? 0;

            var data = this.GetSubconjunto(pagina);

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView("Subconjunto", data);
            }

            return this.View(data);
        }

        //nuevo!! para traer un solo articulo (para cuando linkemos desde fb o tw)
        [ActionName("singlepost")]
        public ActionResult SinglePost(int id)
        {
            var data = (from o in this.DbContext.Blog
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

            this.TempData["DisableScroll"] = true;

            return this.View("Index", data);
        }

        [Authorize]
        public ActionResult Anadir()
        {
            var blog = new BlogVista();
            return this.View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Anadir(BlogVista model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
            {
                if (fichero != null && fichero.ContentLength > 0)
                {
                    var cus = (CustomIdentity)System.Web.HttpContext.Current.User.Identity;
                    var usu = this.DbContext.Usuario.FirstOrDefault(oo => oo.login == cus.Email);

                    var blog = new Blog();

                    if (usu != null)
                    {
                        blog.idUsuario = usu.idUsuario;
                    }

                    blog.titulo = model.Titulo;
                    blog.fecha = model.Fecha;
                    blog.texto = model.Texto;
                    blog.foto = "";
                    blog.profesional = false;
                    this.DbContext.Blog.Add(blog);
                    this.DbContext.SaveChanges();

                    try
                    {
                        var nombre = "Blog" + blog.idBlog;
                        var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", StringComparison.Ordinal));
                        var ruta = this.Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                        fichero.SaveAs(ruta);
                        blog.foto = "~/uploads/fotos/" + nombre + extension;

                        this.DbContext.SaveChanges();

                        this.PublicarEnRedesSociales(blog, nombre, extension);
                    }
                    catch (Exception e)
                    {
                        //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                        this.DbContext.Blog.Remove(blog);
                        this.DbContext.SaveChanges();

                        this.ModelState.AddModelError("", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCode.ErrorAddingItem) + " " + e.Message);
                        return this.View(model);
                    }
                }
            }

            return this.RedirectToAction("Index", "Blog");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var blog = (from o in this.DbContext.Blog
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

            return this.View(blog);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(BlogVista model, HttpPostedFileBase fichero)
        {
            if (model != null && this.ModelState.IsValid)
            {
                //Buscamos el blog a modificar...
                var blog = this.DbContext.Blog.Find(model.IdBlog);

                blog.titulo = model.Titulo;
                blog.texto = model.Texto;
                blog.profesional = false;
                blog.fecha = model.Fecha;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos la nueva imagen con la misma ruta que tenía antes, solo cambia el nombre
                    var nombre = string.Format("Blog{0}", model.IdBlog);
                    var extension = fichero.FileName.Substring(fichero.FileName.LastIndexOf(".", comparisonType: StringComparison.Ordinal));
                    var ruta = this.Server.MapPath("~/Uploads/Fotos") + "/" + nombre + extension;
                    fichero.SaveAs(ruta);
                    blog.foto = "~/uploads/fotos/" + nombre + extension;
                }

                this.DbContext.SaveChanges();
            }

            return this.RedirectToAction("Index", "Blog");
        }

        // action para eliminar una entrada en el blog
        [Authorize]
        public ActionResult Eliminar(int id)
        {
            //buscamos el bicho y lo eliminamos
            if (this.ModelState.IsValid)
            {
                var blog = this.DbContext.Blog.Find(id);

                //borramos la foto
                var foto = blog.foto;
                var rutacompleta = this.Server.MapPath("~/") + foto.Substring(2);
                System.IO.File.Delete(rutacompleta);

                this.DbContext.Blog.Remove(blog);
                this.DbContext.SaveChanges();
            }
            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }

        private List<BlogVista> GetSubconjunto(int pagina = 1)
        {
            if (this.TempData["DisableScroll"] != null && (bool)this.TempData["DisableScroll"])
            {
                this.TempData["DisableScroll"] = true;
                return null;
            }

            var skipRecords = pagina * Elementospp;

            return _conjuntodata.
                Skip(skipRecords).
                Take(Elementospp).ToList();
        }

        private void PublicarEnRedesSociales(Blog blog, string nombre, string extension)
        {
            //la direccion del nuevo post es:
            var direccionnueva = System.Configuration.ConfigurationManager.AppSettings["url_actual"] + "/blog/singlepost/?id=" + blog.idBlog;
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
                    this.ModelState.AddModelError("",
                        Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCode.ErrorPublishingToSocialNetwork) +
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