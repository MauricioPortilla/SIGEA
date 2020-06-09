using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace SIGEABD {
    public partial class Evento {

        /// <summary>
        /// Registra el evento en la base de datos.
        /// </summary>
        /// <returns>true si se registró; false si no</returns>
        public bool Registrar () {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    sigeaBD.Evento.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Evento->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Evento->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Evento->Registrar() -> " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Retorna el nombre del evento.
        /// </summary>
        /// <returns>Nombre del evento</returns>
        public override string ToString() {
            return nombre;
        }
    }
}
