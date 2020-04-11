using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class EvaluarArticuloPruebas {
        /// <summary>
        /// Prueba que verifica que se puede obtener una evaluación artículo
        /// con base en el identificador del artículo y del revisor.
        /// </summary>
        [TestMethod]
        public void ObtenerEvaluacionArticuloExistente() {
            EvaluacionArticulo evaluacionArticulo = null;
            EvaluacionArticulo.ObtenerEvaluacionArticulo(1, 1, (resultado) => {
                evaluacionArticulo = resultado;
            });
            Assert.IsNotNull(evaluacionArticulo);
        }

        /// <summary>
        /// Prueba que verifica que se puede registrar una evaluación artículo
        /// con base en el identificador del revisor y del artículo.
        /// </summary>
        [TestMethod]
        public void RegistrarEvaluacionArticulo() {
            var evaluacionArticulo = new EvaluacionArticulo {
                gradoExpertiz = 2,
                calificacion = 3,
                observaciones = "Muy bien",
                fecha = DateTime.Now,
                estado = "En proceso"
            };
            Assert.IsTrue(evaluacionArticulo.Registrar(1, 1));
        }

        /// <summary>
        /// Prueba que verifica que se pueden actualizar los datos de una
        /// evaluación artículo.
        /// </summary>
        [TestMethod]
        public void ActualizarEvaluacionArticulo() {
            EvaluacionArticulo evaluacionArticulo = null;
            EvaluacionArticulo.ObtenerEvaluacionArticulo(1, 1, (resultado) => {
                evaluacionArticulo = resultado;
            });
            evaluacionArticulo.observaciones = "Argumentos";
            Assert.IsTrue(evaluacionArticulo.Actualizar());
        }
    }
}
