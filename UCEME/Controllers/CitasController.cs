using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UCEME.Models;
using UCEME.Models.ClasesVista;

namespace UCEME.Controllers
{
    public class CitasController : SuperController
    {
        //
        // GET: /Citas/

        public ActionResult Index()
        {
            var cita = new CitaVista();
            //cargamos el combo de hospitales
            CargarCombos();
            return View(cita);
        }

        //action para actualizar el perfil
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult NuevaCita(string hospi, string dia, string diasemana, string hora, string nombre, string telefono, string email, string observaciones)
        {
            try
            {
                var cita = new Cita
                {
                    dia = Convert.ToInt32(dia),
                    hora = Utilidades.DiasHoras.TimeToDecimal(hora),
                    nombre = nombre,
                    telefono = telefono
                };

                var weekday = Convert.ToInt32(diasemana);
                var idHospi = Convert.ToInt32(hospi);
                //nos traemos los turnos que coinciden con la busqueda
                var turno = DbContext.Turno.FirstOrDefault(o => o.idHospital == idHospi && o.dia == weekday);
                cita.Turno = turno;
                if (email != null)
                {
                    //fijamos el email y avisamos al fulano
                    cita.email = email;
                }
                DbContext.Cita.Add(cita);
                Utilidades.Notificaciones.NotificarCitasMedicos(cita, observaciones);
                if (email != null)
                {
                    Utilidades.Notificaciones.NotificarCitasUsuario(cita, observaciones);
                }
                DbContext.SaveChanges();

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaDias(int id)
        {
            var listaDias = DbContext.Turno.Where(o => o.idHospital == id).Select(o => o.dia).ToList();

            return Json(listaDias, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaHoras(string diasem, string hospi, string dia, string mes, string ano)
        {
            var fechalarga = Convert.ToInt32(dia) + (Convert.ToInt32(mes) + 1) * 100 + Convert.ToInt32(ano) * 10000;
            var idHospi = Convert.ToInt32(hospi);
            var weekday = Convert.ToInt32(diasem);

            var listaHoras = new List<string>();

            //nos traemos los turnos que coinciden con la busqueda
            var listaTurnos = DbContext.Turno.Where(o => o.idHospital == idHospi && o.dia == weekday).ToList();
            foreach (var turno in listaTurnos)
            {
                //nos traemos las citas ya asignadas ese dia
                var listacitas = DbContext.Cita.Where(o => o.dia == fechalarga && o.idTurno == turno.idTurno).ToList();
                var salto = 1 / Convert.ToDecimal(turno.porhora);
                //añadimos todas las horas correspondientes
                //hacemos el bucle de las citas paralelas..la lista saldra duplicada, triplicada o lo que sea
                for (var i = 0; i < turno.paralelas; i++)
                {
                    for (var j = turno.inicio; j <= turno.fin; j += salto)
                    {
                        listaHoras.Add(Utilidades.DiasHoras.TimeToString(j));
                    }
                }
                foreach (var cit in listacitas)
                {
                    var cita = Utilidades.DiasHoras.TimeToString(cit.hora);
                    listaHoras.Remove(cita);
                }
            }

            listaHoras = listaHoras.OrderBy(o => o).Distinct().ToList();

            return Json(listaHoras, JsonRequestBehavior.AllowGet);
        }

        private void CargarCombos()
        {
            //cargamos el combo de hospitales

            var listaHospitales = new List<HospitalMinVista>
            {
                new HospitalMinVista { IdDatosPro = 0, Nombre = "Seleccione"}
            };

            //añadimos un elemento que sea el selector y que de paso nos permita quitar el filtro

            //sacamos las marcas y las añadimos a la lista
            var listaHospitalVista = DbContext.DatosProfesionales.Where(x => x.activo.HasValue && x.activo.Value).Select(o => new HospitalMinVista { IdDatosPro = o.idDatosPro, Nombre = o.nombre });
            listaHospitales.AddRange(listaHospitalVista);

            ViewBag.listaHospitales = listaHospitalVista;
            ViewBag.selectListHospitales = new SelectList(listaHospitales, "IdDatosPro", "nombre", "idHospital");
        }
    }
}