using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {

    [TestClass]
    public class ModificarTareasAsignadasPruebas {

        [TestMethod]
        public void ModificarTareasAsignadasPrueba() {
            using(SigeaBD sigeaBD = new SigeaBD()) {
                var tarea = sigeaBD.Tarea.Find(1);
                tarea.descripcion = "Esta tarea fue modificada a modo de prueba";
                Assert.IsTrue(sigeaBD.SaveChanges() != 0);
            }
        }
    }
}
