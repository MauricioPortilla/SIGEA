using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {
    public partial class EvaluacionArticulo {
        public static void ObtenerEvaluacionArticulo(int id_articulo, int id_revisor, Action<EvaluacionArticulo> callbackExitoso) {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    RevisorArticulo revisorArticuloEncontrado = sigeaBD.RevisorArticulo.ToList().Find(
                        revisorArticulo => revisorArticulo.id_articulo == id_articulo && revisorArticulo.id_revisor == id_revisor
                    );
                    if (revisorArticuloEncontrado != null) {
                        EvaluacionArticulo evaluacionArticulo = sigeaBD.EvaluacionArticulo.Find(revisorArticuloEncontrado.id_revisorArticulo);
                        if (evaluacionArticulo != null) {
                            callbackExitoso(evaluacionArticulo);
                        }
                    }
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@EvaluacionArticulo->ObtenerEvaluacionArticulo() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@EvaluacionArticulo->ObtenerEvaluacionArticulo() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@EvaluacionArticulo->ObtenerEvaluacionArticulo() -> " + exception.Message);
                throw;
            }
        }

        public bool Registrar(int id_revisor, int id_articulo) {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    sigeaBD.RevisorArticulo.Add(new RevisorArticulo {
                        id_revisor = id_revisor,
                        id_articulo = id_articulo,
                        EvaluacionArticulo = this
                    });
                    return sigeaBD.SaveChanges() > 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@EvaluacionArticulo->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@EvaluacionArticulo->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@EvaluacionArticulo->Registrar() -> " + exception.Message);
                throw;
            }
        }

        public bool Actualizar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var evaluacionArticulo = sigeaBD.EvaluacionArticulo.Find(id_revisorArticulo);
                    if (evaluacionArticulo == null) {
                        return false;
                    }
                    evaluacionArticulo.gradoExpertiz = gradoExpertiz;
                    evaluacionArticulo.calificacion = calificacion;
                    evaluacionArticulo.observaciones = observaciones;
                    evaluacionArticulo.fecha = fecha;
                    evaluacionArticulo.estado = estado;
                    return sigeaBD.SaveChanges() > 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@EvaluacionArticulo->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@EvaluacionArticulo->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@EvaluacionArticulo->Registrar() -> " + exception.Message);
                throw;
            }
        }
    }
}
