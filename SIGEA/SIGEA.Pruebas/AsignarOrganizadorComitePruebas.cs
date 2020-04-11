using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class AsignarOrganizadorComitePruebas {
        /// <summary>
        /// Prueba que verifica que se puede asignar un organizador a un comité.
        /// </summary>
        [TestMethod]
        public void AsignarOrganizadorPrueba() {
            Comite comite = new Comite {
                id_comite = 1
            };
            using (SigeaBD sigeaBD = new SigeaBD()) {
                var organizador = sigeaBD.Organizador.Find(1);
                Assert.IsTrue(comite.AsignarOrganizador(organizador));
            }
        }
    }
}
