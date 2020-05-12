using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static SIGEA.GenerarConstanciasEvento;

namespace SIGEA.Pruebas {
    [TestClass]
    public class GenerarConstanciasEventoPruebas {
        /// <summary>
        /// Prueba que verifica que se crea una constancia de asistencia
        /// a un evento con éxito.
        /// </summary>
        [TestMethod]
        public void GenerarConstanciaPrueba() {
            var generarConstancia = new GenerarConstanciasEvento(1);
            generarConstancia.DirectorioSeleccionado = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            generarConstancia.GenerarConstancia(new AsistenteTabla {
                Nombre = "Mauricio",
                Paterno = "Cruz",
                Materno = "Portilla"
            });
            var nombreAsistente = "Cruz Portilla Mauricio";
            Assert.IsTrue(File.Exists(generarConstancia.DirectorioSeleccionado + "/" + nombreAsistente + ".png"));
        }
    }
}
