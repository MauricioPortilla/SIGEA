using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {

    [TestClass]
    public class AsignarArticuloActividadPruebas {

        [TestMethod]
        public void AsignarArticuloActividadPrueba() {
            using(SigeaBD sigeaBD = new SigeaBD()) {
                var articulo = sigeaBD.Articulo.Find(1);
                var presentacion = sigeaBD.Presentacion.Find(1);
                presentacion.Articulo.Add(articulo);
                Assert.IsTrue(sigeaBD.SaveChanges() != 0);
            }
        }
    }
}
