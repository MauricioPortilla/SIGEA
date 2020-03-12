using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class RegistrarActividadPruebas {
        /// <summary>
        /// Prueba que verifica que se registra una actividad para un evento.
        /// </summary>
        [TestMethod]
        public void RegistrarActividadPrueba() {
            Actividad actividad = new Actividad {
                nombre = "Actividad 1",
                descripcion = "Descripción actividad 1",
                tipo = "Tipo actividad 1",
                id_evento = 1
            };
            Assert.IsTrue(actividad.Registrar());
        }
    }
}
