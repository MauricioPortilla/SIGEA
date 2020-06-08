using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace SIGEABD {
    public partial class Track {

        /// <summary>
        /// Registra el track en la base de datos.
        /// </summary>
        /// <returns>true si se registró con éxito; false si no</returns>
        public bool Registrar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    sigeaBD.Track.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Track->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Track->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Track->Registrar() -> " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Retorna el nombre del track.
        /// </summary>
        /// <returns>Nombre del track</returns>
        public override string ToString() {
            return nombre;
        }
    }
}
