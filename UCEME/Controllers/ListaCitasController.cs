using System.Linq;
using System.Web.Mvc;
using UCEME.Models.ClasesVista;

namespace UCEME.Controllers
{
    public class ListaCitasController : SuperController
    {
        //
        // GET: /ListaCitas/

        [Authorize]
        public ActionResult Index()
        {
            var data = DbContext.Cita.OrderBy(o => o.Turno.idHospital).ThenBy(o => o.dia).ThenBy(o => o.hora)
                .Select(o => new CitaVista { IdCita = o.idCita, Dia = o.dia, Hora = o.hora, Nombre = o.nombre, Email = o.email, Telefono = o.telefono, Hospital = o.Turno.DatosProfesionales.nombre, IdTurno = o.idTurno });
            return View(data);
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var cita = DbContext.Cita.Find(id);

            DbContext.Cita.Remove(cita);

            DbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}