﻿namespace UCEME.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Uceme.Model.Models.ClasesVista;

    public class ListaCitasController : SuperController
    {
        //
        // GET: /ListaCitas/

        [Authorize]
        public ActionResult Index()
        {
            var data = this.DbContext.Cita.OrderBy(o => o.Turno.idHospital).ThenBy(o => o.dia).ThenBy(o => o.hora)
                .Select(o => new CitaVista { IdCita = o.idCita, Dia = o.dia, Hora = o.hora, Nombre = o.nombre, Email = o.email, Telefono = o.telefono, Hospital = o.Turno.DatosProfesionales.nombre, IdTurno = o.idTurno });
            return this.View(data);
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var cita = this.DbContext.Cita.Find(id);

            this.DbContext.Cita.Remove(cita);

            this.DbContext.SaveChanges();

            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}