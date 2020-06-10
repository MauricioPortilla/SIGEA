using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {

    [TestClass]
    public class AsignarArticuloRevisorPruebas {

        [TestMethod]
        public void AsignarArticuloRevisorPrueba() {
            using(SigeaBD sigeaBD = new SigeaBD()) {
                var articulo = sigeaBD.Articulo.Find(1);
                var revisor = sigeaBD.Revisor.Find(1);
                sigeaBD.RevisorArticulo.Add(new RevisorArticulo { 
                    id_articulo = articulo.id_articulo,
                    id_revisor = revisor.id_revisor
                });
                Assert.IsTrue(sigeaBD.SaveChanges() != 0);
            }
        }
    }
}
