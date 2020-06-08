using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SIGEABD {
    public partial class Tarea {
        /// <summary>
        /// Registra la tarea en la base de datos.
        /// </summary>
        /// <returns>true si se registró con éxito; false si no</returns>
        public bool Registrar() {
			try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    if (Actividad.Count > 0) {
                        sigeaBD.Actividad.Attach(Actividad.First());
                    }
                    sigeaBD.Tarea.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Tarea->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Tarea->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Tarea->Registrar() -> " + exception.Message);
                throw;
            }
        }
    }
}
