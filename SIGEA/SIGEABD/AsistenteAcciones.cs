using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace SIGEABD {
    public partial class Asistente {
        public bool Registrar () {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
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
