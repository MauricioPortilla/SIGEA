using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class GenerarReporteIngresosActividadPruebas {
        [TestMethod]
        public void GenerarReportePrueba() {
            var generarReporte = new GenerarReporteIngresosActividad(new Actividad {
                id_actividad = 1,
                nombre = "Actividad 1"
            });
            generarReporte.RutaSeleccionada = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            generarReporte.GenerarReporte(new List<List<string>>() {
                new List<string>() {
                    "20/10/2020", "260"
                },
                new List<string>() {
                    "10/10/2020", "220"
                }
            }, 1);
            Assert.IsTrue(File.Exists(generarReporte.RutaSeleccionada + "/ReporteActividad_Actividad 1_1.png"));
        }
    }
}
