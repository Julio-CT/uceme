namespace UCEME.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using UCEME.Models;
    using UCEME.Models.ClasesVista;

    public class ConocenosController : SuperController
    {
        public ActionResult Index()
        {
            //nos traemos el usuario
            var usrs = (from o in DbContext.Usuario
                        where o.idRol == 2
                        orderby o.display_order descending
                        select o);

            //lo metemos todo
            if (usrs != null && usrs.Any())
            {
                var data = new List<MedicoVista>();
                foreach (var usr in usrs)
                {
                    var dataItem = new MedicoVista
                    {
                        IdUsuario = usr.idUsuario,
                        Nombre = usr.nombre,
                        Apellidos = usr.apellidos,
                        Foto = usr.foto,
                        Login = usr.login,
                        IdCurriculum = usr.idCurriculum,
                        IdDatosContacto = usr.idDatosContacto,
                        Telefono = usr.DatosContacto.telefono,
                        Linkedin = usr.linkedin,
                        Curriculum = new Curriculum
                        {
                            Titulo = usr.Curriculum.Titulo
                        }
                    };

                    //por el lazy loading, metemos el curriculum a manuji
                    foreach (var item in usr.Curriculum.ItemCurriculum)
                    {
                        dataItem.Curriculum.ItemCurriculum.Add(item);
                    }

                    data.Add(dataItem);
                }

                ViewBag.ShowEditButton = false;
                return View(data);
            }

            //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
            return Json("error buscando usuario", JsonRequestBehavior.AllowGet);
        }
    }
}