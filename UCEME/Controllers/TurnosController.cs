using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UCEME.Models;
using UCEME.Models.ClasesVista;
using UCEME.Utilidades;

namespace UCEME.Controllers
{
    public class TurnosController : SuperController
    {
        //
        // GET: /Turnos/
        [Authorize]
        public ActionResult Index()
        {
            var data = DbContext.Turno.OrderBy(o => o.idHospital).ThenBy(o => o.dia).ThenBy(o => o.inicio)
                .Select(o => new TurnoVista { IdTurno = o.idTurno, IdHospital = o.idHospital, Dia = o.dia, Inicio = o.inicio, Fin = o.fin, Paralelas = o.paralelas, Porhora = o.porhora, Hospital = o.DatosProfesionales.nombre });

            //cargamos el combo de hospitales
            CargarCombos();
            return View(data);
        }

        [Authorize]
        public ActionResult EditItem(int idTurno, string dia, string paralelas, string porhora, string strinicio, string strfin)
        {
            var turno = DbContext.Turno.Find(idTurno);

            turno.dia = Convert.ToInt32(dia);
            turno.inicio = DiasHoras.TimeToDecimal(strinicio);
            turno.fin = DiasHoras.TimeToDecimal(strfin);
            turno.paralelas = Convert.ToInt32(paralelas);
            turno.porhora = Convert.ToInt32(porhora);

            //si dejamos alguna cita fuera...debemos avisar
            var listacitas = DbContext.Cita.Where(o => o.idTurno == idTurno).ToList();
            listacitas = listacitas.Where(o => o.hora > turno.fin || o.hora < turno.inicio).ToList();

            foreach (var cita in listacitas)
            {
                if (Notificaciones.ModificarCitasMedicos(cita))
                {
                    DbContext.Cita.Remove(cita);
                }
                else
                {
                    //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
            }

            DbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DeleteItem(int idTurno)
        {
            var turno = DbContext.Turno.Find(idTurno);

            //nos cargamos las citas que tenia ese turno
            var listacitas = DbContext.Cita.Where(o => o.idTurno == idTurno).ToList();

            foreach (var cita in listacitas)
            {
                if (Notificaciones.ModificarCitasMedicos(cita))
                {
                    DbContext.Cita.Remove(cita);
                }
                else
                {
                    //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
            }
            DbContext.Turno.Remove(turno);

            DbContext.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        //action para actualizar el perfil
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult NewItem(FormCollection coleccion)
        {
            var cuantos = coleccion.Count;
            var idHosp = Convert.ToInt32(coleccion.Get(0));
            var inicio = Utilidades.DiasHoras.TimeToDecimal(coleccion.Get(cuantos - 4));
            var fin = Utilidades.DiasHoras.TimeToDecimal(coleccion.Get(cuantos - 3));
            var paralelas = Convert.ToInt32(coleccion.Get(cuantos - 2));
            var porhora = Convert.ToInt32(coleccion.Get(cuantos - 1));

            for (var i = 1; i < cuantos - 4; i++)
            {
                var turno = new Turno
                {
                    idHospital = idHosp,
                    dia = Convert.ToInt32(coleccion.GetKey(i)),
                    inicio = inicio,
                    fin = fin,
                    paralelas = paralelas,
                    porhora = porhora
                };

                DbContext.Turno.Add(turno);
            }
            DbContext.SaveChanges();

            return RedirectToAction("index");
        }

        private void CargarCombos()
        {
            //cargamos el combo de hospitales

            var listaHospitales = new List<HospitalMinVista>
            {
                new HospitalMinVista {IdDatosPro = 0, Nombre = "Selecciona Hospital"}
            };
            //añadimos un elemento que sea el selector y que de paso nos permita quitar el filtro

            //sacamos las marcas y las añadimos a la lista
            var ieHospi = DbContext.DatosProfesionales.Where(x => x.activo.HasValue && x.activo.Value).Select(o => new HospitalMinVista { IdDatosPro = o.idDatosPro, Nombre = o.nombre });
            listaHospitales.AddRange(ieHospi);

            ViewBag.idHospital = new SelectList(listaHospitales, "IdDatosPro", "nombre", "idHospital");
        }
    }
}