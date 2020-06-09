using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace SIGEABD {

    public partial class Magistral {

        /// <summary>
        /// Registra al magistral en la base de datos.
        /// </summary>
        /// <returns>true si se registró con éxito; false si no</returns>
        public bool Registrar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    sigeaBD.Magistral.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Magistral->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Magistral->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Magistral->Registrar() -> " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Retorna el nombre completo del magistral.
        /// </summary>
        /// <returns>Nombre completo del magistral</returns>
        public override string ToString() {
            return nombre + " " + paterno + " " + (materno ?? "");
        }
    }
}
