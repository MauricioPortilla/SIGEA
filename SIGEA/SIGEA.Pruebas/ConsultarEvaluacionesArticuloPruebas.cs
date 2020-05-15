using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class ConsultarEvaluacionesArticuloPruebas {
        /// <summary>
        /// Prueba que verifica que se puede actualizar el
        /// estado de un artículo.
        /// </summary>
        [TestMethod]
        public void ActualizarEstadoArticuloPrueba() {
            Articulo articulo = new Articulo {
                id_articulo = 1
            };
            articulo.estado = "Aceptado";
            Assert.IsTrue(articulo.Actualizar());
        }
    }
}
