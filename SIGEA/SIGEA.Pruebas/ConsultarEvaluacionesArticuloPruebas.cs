using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class ConsultarEvaluacionesArticuloPruebas {
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
