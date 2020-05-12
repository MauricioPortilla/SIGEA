using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SIGEA.Pruebas {
    [TestClass]
    public class GenerarProgramaEventoPruebas {
        [TestMethod]
        public void GenerarProgramaPrueba() {
            var generarPrograma = new GenerarProgramaEvento(new SIGEABD.Evento { id_evento = 1, nombre = "Evento 1" });
            generarPrograma.RutaSeleccionada = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            generarPrograma.GenerarPrograma(new List<List<string>>() {
                new List<string>() {
                    "Actividad 1", "20/10/2020", "10:00:00", "12:00:00"
                }
            }, 1);
            Assert.IsTrue(File.Exists(generarPrograma.RutaSeleccionada + "/ProgramaEvento_Evento 1_1.png"));
        }
    }
}
