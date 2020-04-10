using System.Linq;
using System.Web.Mvc;
using Uceme.Model.Models.ClasesVista;

namespace UCEME.Controllers
{
    public class HomeController : SuperController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Unidad de Cirugía Endocrinometabólica Especializada.";

            var data = (from o in DbContext.Usuario
                        where o.idRol == 2
                        orderby o.display_order
                        select new MedicoMinVista
                        {
                            IdUsuario = o.idUsuario,
                            Nombre = o.nombre,
                            Apellidos = o.apellidos,
                            Foto = o.foto,
                            Titulo = o.Curriculum.Titulo,
                            Posicion = o.display_order
                        }).ToList();

            for (var i = 0; i < data.Count; i++)
            {
                data.ElementAt(i).Posicion = i;
            }

            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "UCEME Web Application.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MostrarFotos()
        {
            var listaFotos = (from o in DbContext.Fotos
                              where o.destacada.Value
                              select new FotosVista
                              {
                                  IdFoto = o.idFoto,
                                  Nombre = o.nombre,
                                  Texto = o.texto
                              }).ToList();

            return View(listaFotos);
        }
    }
}