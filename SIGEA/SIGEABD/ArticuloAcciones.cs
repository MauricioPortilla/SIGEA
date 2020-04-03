using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {
    public partial class Articulo {

        public static void ObtenerArticulo(int id_articulo, Action<Articulo> callbackExito) {
            try {
                Articulo articulo = null;
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    articulo = sigeaBD.Articulo.Find(id_articulo);
                    callbackExito(articulo);
                }
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Articulo->ObtenerArticulo() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Articulo->ObtenerArticulo() -> " + exception.Message);
                throw;
            }
        }

        public bool Registrar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    foreach (var autor in AutorArticulo) {
                        sigeaBD.Autor.Attach(autor.Autor);
                    }
                    sigeaBD.Track.Attach(Track);
                    sigeaBD.Articulo.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Articulo->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Articulo->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Articulo->Registrar() -> " + exception.Message);
                throw;
            }
        }
    }
}
