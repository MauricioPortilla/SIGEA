using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static SIGEA.GenerarConstanciasActividad;

namespace SIGEA.Pruebas {
    [TestClass]
    public class GenerarConstanciasActividadPruebas {
        [TestMethod]
        public void GenerarConstanciasActividadPrueba() {
            var generarConstancia = new GenerarConstanciasActividad(1);
            generarConstancia.DirectorioSeleccionado = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            generarConstancia.GenerarConstancia(new AsistenteTabla {
                Nombre = "Juan Carlos",
                Paterno = "Suarez",
                Materno = "Hernandez"
            });
            var nombreAsistente = "Suarez Hernández Juan Carlos";
            Assert.IsTrue(File.Exists(generarConstancia.DirectorioSeleccionado + "/" + nombreAsistente + ".png"));
        }
    }
}
