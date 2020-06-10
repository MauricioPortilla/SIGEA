using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {

    [TestClass]
    public class ConsultarAsistentesEventoPruebas {

        [TestMethod]
        public void ConsultarAsistentesEventoPrueba() {
            using(SigeaBD sigeaBD = new SigeaBD()) {
                var evento = sigeaBD.Evento.Find(1);
                var asistentes = evento.Asistente.Where(
                    asistente => asistente.Pago != null);
                Assert.IsTrue(asistentes.Count() != 0);
            }
        }
    }
}
