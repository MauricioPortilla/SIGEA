using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class ModificarActtividadPruebas {
        /// <summary>
        /// Prueba que verifica que una actividad puede ser actualizada.
        /// </summary>
        [TestMethod]
        public void ActualizarActividadPrueba() {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                var actividad = sigeaBD.Actividad.Find(1);
                actividad.descripcion = "Descripción nueva";
                Assert.IsTrue(actividad.Actualizar());
            }
        }
    }
}
