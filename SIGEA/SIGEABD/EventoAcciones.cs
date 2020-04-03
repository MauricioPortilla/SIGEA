using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace SIGEABD {
    public partial class Evento {
        public override string ToString() {
            return nombre;
        }

        public bool Registrar () {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    sigeaBD.Evento.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Articulo->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Articulo->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Adscripcion->Registrar() -> " + exception.Message);
                throw;
            }
        }
    }
}
