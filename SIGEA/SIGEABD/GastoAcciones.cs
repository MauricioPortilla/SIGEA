using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {
    public partial class Gasto {
        /// <summary>
        /// Registra el gasto en la base de datos.
        /// </summary>
        /// <returns>true si se registró con éxito; false si no</returns>
        public bool Registrar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    if (Magistral.Count > 0) {
                        sigeaBD.Magistral.Attach(Magistral.First());
                    }
                    sigeaBD.Gasto.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Gasto->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Gasto->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Gasto->Registrar() -> " + exception.Message);
                throw;
            }
        }
    }
}
