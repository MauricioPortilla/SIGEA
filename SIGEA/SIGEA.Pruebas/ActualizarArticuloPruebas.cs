using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class ActualizarArticuloPruebas {
        /// <summary>
        /// Prueba que verifica que se puede obtener un artículo con base
        /// en su identificador.
        /// </summary>
        [TestMethod]
        public void CargarArticuloExistentePrueba() {
            Articulo.ObtenerArticulo(1, (articulo) => {
                Assert.IsNotNull(articulo);
            });
        }

        /// <summary>
        /// Prueba que verifica que la búsqueda de un artículo no existente
        /// con base en su identificador devuelve nulo.
        /// </summary>
        [TestMethod]
        public void CargarArticuloNoExistentePrueba() {
            Articulo.ObtenerArticulo(10, (articulo) => {
                Assert.IsNull(articulo);
            });
        }
    }
}
