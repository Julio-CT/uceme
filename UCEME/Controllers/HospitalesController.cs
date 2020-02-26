namespace UCEME.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using UCEME.Models;
    using UCEME.Models.ClasesVista;

    public class HospitalesController : SuperController
    {
        public ActionResult Index()
        {
            var hospi = DbContext.DatosProfesionales.Where(x => x.activo.HasValue && x.activo.Value).Select(o => o);

            var data = new List<HospitalesVista>();
            foreach (var o in hospi)
            {
                var item = new HospitalesVista
                {
                    IdHospital = o.idDatosPro.ToString(),
                    Nombre = o.nombre,
                    Telefono = o.telefono,
                    Email = o.email,
                    Direccion = o.direccion,
                    Texto = o.texto,
                    Foto = o.foto
                };

                item.Companies = new List<CompaniasVista>();

                //comprobamos, como siempre
                if (o.Companias != null)
                {
                    var pos = 0;
                    foreach (var oo in o.Companias)
                    {
                        var company = new CompaniasVista
                        {
                            IdCompanias = oo.idCompanias,
                            Nombre = oo.nombre,
                            Link = oo.link,
                            Posicion = pos,
                            Foto = oo.foto
                        };

                        item.Companies.Add(company);
                        pos++;
                    }
                }

                data.Add(item);
            }

            return View(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult CrearHospital(string nombre, string telefono, string email, string direccion, string texto, HttpPostedFileBase fichero)
        {
            var hop = new DatosProfesionales
            {
                nombre = nombre,
                telefono = telefono,
                direccion = direccion,
                email = email,
                texto = texto
            };

            DbContext.DatosProfesionales.Add(hop);
            DbContext.SaveChanges();

            if (fichero != null && fichero.ContentLength > 0)
            {
                try
                {
                    //guardamos el fichero de la foto con nombre hosp + id
                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.Ordinal));
                    var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\hosp" + hop.idDatosPro + extension;
                    fichero.SaveAs(rutacompleta);
                    hop.foto = "~/uploads/fotos/hosp" + hop.idDatosPro + extension;
                }
                catch (Exception e)
                {
                    //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                    DbContext.DatosProfesionales.Remove(hop);
                    DbContext.SaveChanges();

                    ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                    return RedirectToAction("index", "Hospitales");
                }
            }

            DbContext.SaveChanges();

            return RedirectToAction("index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult CrearCompania(string nombre, string link, HttpPostedFileBase fichero)
        {
            var comp = new Companias
            {
                nombre = nombre,
                link = link,
                foto = ""
            };

            DbContext.Companias.Add(comp);
            DbContext.SaveChanges();

            if (fichero != null && fichero.ContentLength > 0)
            {
                try
                {
                    //guardamos el fichero de la foto con nombre comp + id
                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.CurrentCulture));
                    var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\comp" + comp.idCompanias + extension;
                    fichero.SaveAs(rutacompleta);
                    comp.foto = "~/uploads/fotos/comp" + comp.idCompanias + extension;
                }
                catch (Exception e)
                {
                    //si falla el anadir la foto, borramos el elemento de la base de datos y devolvemos la vista con un error
                    DbContext.Companias.Remove(comp);
                    DbContext.SaveChanges();

                    ModelState.AddModelError("UcemeError", Utilidades.ErrorManager.ErrorCodeToString(Utilidades.ErrorCodes.ErrorAddingItem) + " " + e.Message);
                    return RedirectToAction("index", "Hospitales");
                }
            }

            DbContext.SaveChanges();

            return RedirectToAction("index");
        }

        [Authorize]
        public ActionResult DeleteHospital(string id)
        {
            try
            {
                var hospitalABorrar = DbContext.DatosProfesionales.Find(id);

                //nos debemos cargar todos los turnos que le cuelgan
                var listaturnos = hospitalABorrar.Turno.ToList();
                foreach (var turno in listaturnos)
                {
                    var idTurno = turno.idTurno;
                    //nos cargamos las citas que tenia ese turno
                    var listacitas = DbContext.Cita.Where(o => o.idTurno == idTurno).ToList();

                    foreach (var cita in listacitas)
                    {
                        if (UCEME.Utilidades.Notificaciones.ModificarCitasMedicos(cita))
                        {
                            DbContext.Cita.Remove(cita);
                        }
                        else
                        {
                            //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                            return Json("error eliminando citas", JsonRequestBehavior.AllowGet);
                        }
                    }
                    //nos cargamos el turno
                    DbContext.Turno.Remove(turno);
                }

                //borramos la foto
                var foto = hospitalABorrar.foto;
                var rutacompleta = Server.MapPath("~/") + foto.Substring(2);
                try
                {
                    System.IO.File.Delete(rutacompleta);
                }
                catch { }

                //nos cargamos todas previamente
                foreach (var co in hospitalABorrar.Companias.ToList())
                {
                    hospitalABorrar.Companias.Remove(co);
                }

                //nos cargamos el hospital
                hospitalABorrar.activo = false;
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                return Json(string.Format("error eliminando hospital {0}", e.Message), JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("index");
        }

        [Authorize]
        public ActionResult DeleteComp(int id)
        {
            try
            {
                var companiaABorrar = DbContext.Companias.Find(id);

                //nos debemos cargar todos los hospitales que le cuelgan
                var listahospitales = companiaABorrar.DatosProfesionales.ToList();

                foreach (DatosProfesionales hospital in listahospitales)
                {
                    hospital.Companias.Remove(companiaABorrar);
                }

                //borramos la foto
                var foto = companiaABorrar.foto;
                var rutacompleta = Server.MapPath("~/") + foto.Substring(2);
                try
                {
                    System.IO.File.Delete(rutacompleta);
                }
                catch { }

                //nos cargamos el hospital
                DbContext.Companias.Remove(companiaABorrar);
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                return Json(string.Format("error eliminando compañia {0}", e.Message), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("index");
        }

        [Authorize]
        public ActionResult EditarHospital(string id)
        {
            try
            {
                var hospitalAEditar = new DatosProfesionales { idDatosPro = Convert.ToInt32(id) };
                var hospital =
                    (DbContext.DatosProfesionales.Where(o => o.idDatosPro == hospitalAEditar.idDatosPro)
                        .Select(o => new HospitalesVista
                        {
                            IdHospital = id,
                            Nombre = o.nombre,
                            Telefono = o.telefono,
                            Email = o.email,
                            Direccion = o.direccion,
                            Texto = o.texto,
                            Foto = o.foto
                        })).FirstOrDefault();


                var hospitalCompania = new DatosProfesionales { idDatosPro = Convert.ToInt32(id) };
                hospitalCompania = DbContext.DatosProfesionales.Find(hospitalCompania.idDatosPro);
                var listaCompanias = hospitalCompania.Companias.Select(o => o.idCompanias);

                //sacamos las categorias y las añadimos a la lista
                List<CompaniasSelectorVista> ieCompanias = (DbContext.Companias.Select(comp => new CompaniasSelectorVista
                {
                    IdCompanias = comp.idCompanias,
                    Nombre = comp.nombre,
                    Checked = listaCompanias.Contains(comp.idCompanias)
                })).ToList();

                hospital.ListaCompanias = ieCompanias;

                return View(hospital);
            }
            catch (Exception e)
            {
                //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                return Json(string.Format("error editando hospital {0}", e.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult EditarHospital(HospitalesVista model, HttpPostedFileBase fichero, FormCollection coleccion)
        {
            try
            {
                var hospitalAEditar = new DatosProfesionales { idDatosPro = Convert.ToInt32(model.IdHospital) };
                hospitalAEditar = DbContext.DatosProfesionales.Find(hospitalAEditar.idDatosPro);

                hospitalAEditar.nombre = model.Nombre;
                hospitalAEditar.telefono = model.Telefono;
                hospitalAEditar.direccion = model.Direccion;
                hospitalAEditar.email = model.Email;
                hospitalAEditar.texto = model.Texto;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos el fichero de la foto con nombre hosp + id
                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.CurrentCulture));
                    var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\" + model.IdHospital + extension;
                    fichero.SaveAs(rutacompleta);
                    hospitalAEditar.foto = "~/uploads/fotos/" + model.IdHospital + extension;
                }

                //nos cargamos todas previamente
                foreach (var co in hospitalAEditar.Companias.ToList())
                {
                    hospitalAEditar.Companias.Remove(co);
                }

                for (var i = 6; i < coleccion.Count; i++)
                {
                    var id = Convert.ToInt32(coleccion.GetKey(i));
                    var co = DbContext.Companias.Find(id);
                    hospitalAEditar.Companias.Add(co);
                }

                DbContext.SaveChanges();

                return RedirectToAction("index");
            }
            catch (Exception e)
            {
                //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                return Json(string.Format("error editando hospital {0}", e.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult EditarCompania(int id)
        {
            var data = DbContext.Companias.Find(id);

            return View(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult EditarCompania(Companias model, HttpPostedFileBase fichero)
        {
            try
            {
                var comp = DbContext.Companias.Find(model.idCompanias);

                comp.nombre = model.nombre;
                comp.link = model.link;

                if (fichero != null && fichero.ContentLength > 0)
                {
                    //guardamos el fichero de la foto con nombre comp + id
                    var nombreFichero = fichero.FileName;
                    var extension = nombreFichero.Substring(nombreFichero.LastIndexOf(".", StringComparison.CurrentCulture));
                    var rutacompleta = Server.MapPath("~/uploads/fotos") + @"\comp" + comp.idCompanias + extension;
                    fichero.SaveAs(rutacompleta);
                    comp.foto = "~/uploads/fotos/comp" + comp.idCompanias + extension;
                }

                DbContext.SaveChanges();

                return RedirectToAction("index");
            }
            catch (Exception e)
            {
                //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                return Json(string.Format("error editando compañia {0}", e.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaCompanias(string id)
        {
            try
            {
                var hospital = new DatosProfesionales { idDatosPro = Convert.ToInt32(id) };
                hospital = DbContext.DatosProfesionales.Find(hospital.idDatosPro);
                var listaCompanias = hospital.Companias.Select(o => o.idCompanias);

                return Json(listaCompanias, JsonRequestBehavior.AllowGet);
            }
#pragma warning disable CS0168 // La variable 'e' se ha declarado pero nunca se usa
            catch (Exception e)
#pragma warning restore CS0168 // La variable 'e' se ha declarado pero nunca se usa
            {
                //algo deberiamos hacer si falla..pero si no podemos enviar un email...chungo..
                return Json("error listando compañias", JsonRequestBehavior.AllowGet);
            }
        }
    }
}