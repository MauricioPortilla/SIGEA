using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace SIGEABD {

    public partial class Magistral {

        public bool Registrar () {

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
    }
}
