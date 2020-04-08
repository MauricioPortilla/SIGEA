using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {
    public partial class Actividad {
        /// <summary>
        /// Registra la actividad en la base de datos.
        /// </summary>
        /// <returns>true si se registró correctamente; false si no</returns>
        public bool Registrar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    sigeaBD.Evento.Attach(Evento);
                    sigeaBD.Actividad.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Actividad->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Actividad->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Actividad->Registrar() -> " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualiza la actividad en la base de datos.
        /// </summary>
        /// <returns>true si se registra correctamente; false si no</returns>
        public bool Actualizar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var actividadExistente = sigeaBD.Actividad.Find(id_actividad);
                    actividadExistente.nombre = nombre;
                    actividadExistente.costo = costo;
                    actividadExistente.descripcion = descripcion;
                    actividadExistente.tipo = tipo;
                    List<Presentacion> presentacionesEliminadas = new List<Presentacion>();
                    foreach (var presentacion in actividadExistente.Presentacion) {
                        var existePresentacion = Presentacion.FirstOrDefault(presentacionActual => presentacionActual.id_presentacion == presentacion.id_presentacion);
                        if (existePresentacion == null) {
                            presentacionesEliminadas.Add(presentacion);
                        } else {
                            presentacion.fechaPresentacion = existePresentacion.fechaPresentacion;
                            presentacion.horaInicio = existePresentacion.horaInicio;
                            presentacion.horaFin = existePresentacion.horaFin;
                        }
                    }
                    foreach (var presentacionEliminada in presentacionesEliminadas) {
                        actividadExistente.Presentacion.Remove(presentacionEliminada);
                        sigeaBD.Presentacion.Remove(presentacionEliminada);
                    }
                    foreach (var presentacion in Presentacion) {
                        if (presentacion.id_presentacion == 0) {
                            actividadExistente.Presentacion.Add(presentacion);
                        }
                    }
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Actividad->Actualizar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Actividad->Actualizar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Actividad->Actualizar() -> " + exception.Message);
                throw;
            }
        }
    }
}
