using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class ConsultarGastosPruebas {
        /// <summary>
        /// Prueba que verifica que se obtienen los gastos de un evento.
        /// </summary>
        [TestMethod]
        public void ConsultarGastosPrueba() {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                var gastos = sigeaBD.Gasto.Where(gasto => gasto.id_evento == 1);
                Assert.IsTrue(gastos.Count() > 0);
            }
        }
    }
}
