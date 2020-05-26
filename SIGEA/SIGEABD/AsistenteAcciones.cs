using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace SIGEABD {
    public partial class Asistente {
        public bool Registrar () {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    Collection<Actividad> actividades = new Collection<Actividad>();
                    foreach (var actividad in Actividad) {
                        actividades.Add(sigeaBD.Actividad.Find(actividad.id_actividad));
                    }
                    Actividad = actividades;
                    Collection<Evento> eventos = new Collection<Evento>();
                    foreach (var evento in Evento) {
                        eventos.Add(sigeaBD.Evento.Find(evento.id_evento));
                    }
                    Evento = eventos;
                    sigeaBD.Asistente.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Asistente->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Asistente->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Asistente->Registrar() -> " + exception.Message);
                throw;
            }
        }
    }
}
